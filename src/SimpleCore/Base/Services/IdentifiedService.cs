using Microsoft.EntityFrameworkCore;
using SimpleCore.Base.Entities;
using SimpleCore.Contexts;
using SimpleCore.Data;
using SimpleCore.Identities.Entities;

namespace SimpleCore.Base.Services;

public interface IIdentifiedService<TEntity> : IService<TEntity>
    where TEntity : UserEntity
{
}

/// <inheritdoc/>
public class IdentifiedService<TIdentity, TEntity, TKey> : Service<TEntity>, IIdentifiedService<TEntity>
    where TIdentity : Identity, new()
    where TEntity : UserEntity
{
    protected readonly IIdentityContext<TIdentity> _identityContext;

    public IdentifiedService(SimpleDbContext context, IIdentityContext<TIdentity> identityContext) : base(context)
    {
        _identityContext = identityContext;
    }

    /// <inheritdoc/>
    protected override IQueryable<TEntity> PrepareQuery(bool tracking = false, bool deleted = false)
    {
        var query = base.PrepareQuery(tracking, deleted);

        var identity = _identityContext.GetUserInfo().Result;

        query = query.Where(x => x.OwnerId!.Equals(identity.Id));

        return query;
    }

    /// <inheritdoc/>
    public override async Task Insert(TEntity entity)
    {
        var identity = await _identityContext.GetUserInfo();

        entity.OwnerId = identity.Id;
        entity.CreatorId = identity.Id;

        await base.Insert(entity);
    }

    /// <inheritdoc/>
    public override async Task<TEntity?> GetById(string id, bool tracking = false)
    {
        var identity = await _identityContext.GetUserInfo();

        return await AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id) && x.OwnerId!.Equals(identity.Id));
    }
}
