using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SimpleCore.Base.Entities;
using SimpleCore.Data;
using SimpleCore.Types;
using System.Security.Claims;

namespace SimpleCore.Base.Services;

public interface IUserService<TEntity> : IService<TEntity>
    where TEntity : UserEntity
{

}

/// <inheritdoc/>
public class UserService<TEntity> : Service<TEntity>, IUserService<TEntity>
    where TEntity : UserEntity
{
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected readonly ClaimsPrincipal UserIdentity;
    protected readonly string UserIdentifier;

    public UserService(SimpleDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        HttpContextAccessor = httpContextAccessor;

        if (HttpContextAccessor.HttpContext == null)
            throw new NullReferenceException($"{nameof(HttpContextAccessor.HttpContext)} is null.");

        UserIdentity = HttpContextAccessor.HttpContext.User;

        UserIdentifier = UserIdentity.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new NullReferenceException($"User Identifier is null.");
    }

    /// <inheritdoc/>
    public override Task<TEntity?> GetById(string id, bool tracking = false)
    {
        return AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id) && x.OwnerId!.Equals(UserIdentifier));
    }

    /// <inheritdoc/>
    public override Task Insert(TEntity entity)
    {
        entity.OwnerId = UserIdentifier;
        entity.CreatorId = UserIdentifier;

        return base.Insert(entity);
    }

    /// <inheritdoc/>
    protected override IQueryable<TEntity> PrepareQuery(bool tracking = false, bool deleted = false)
    {
        var query = base.PrepareQuery(tracking, deleted);

        query = query.Where(x => x.OwnerId!.Equals(UserIdentifier));

        return query;
    }
}