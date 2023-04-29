using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SimpleCore.Abstractions;
using SimpleCore.Data;
using SimpleCore.Entities;
using SimpleCore.Types;
using System.Security.Claims;

namespace SimpleCore.Services
{
    /// <inheritdoc/>
    public class UserService<TEntity, TKey> : Service<TEntity, TKey>, IUserService<TEntity, TKey>
        where TEntity : UserEntity<TKey>
    {
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly ClaimsPrincipal UserIdentity;
        protected readonly TKey UserIdentifier;

        public UserService(SimpleDbContext<TKey> context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            HttpContextAccessor = httpContextAccessor;

            if (HttpContextAccessor.HttpContext == null )
                throw new NullReferenceException($"{nameof(HttpContextAccessor.HttpContext)} is null.");

            UserIdentity = HttpContextAccessor.HttpContext.User;

            var identifier = UserIdentity.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? throw new NullReferenceException($"User Identifier is null.");
            
            UserIdentifier = identifier.ChangeType<TKey>(); ;
        }

        /// <inheritdoc/>
        public override Task<TEntity?> GetById(TKey id, bool tracking = false)
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
}