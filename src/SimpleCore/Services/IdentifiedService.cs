using Microsoft.EntityFrameworkCore;
using SimpleCore.Abstractions;
using SimpleCore.Contexts;
using SimpleCore.Data;
using SimpleCore.Entities;
using SimpleCore.Identity;

namespace SimpleCore.Services
{
    /// <inheritdoc/>
    public class IdentifiedService<TIdentity, TEntity, TKey> : Service<TEntity, TKey>, IIdentifiedService<TEntity, TKey>
        where TIdentity : Identity<TKey>, new()
        where TEntity : UserEntity<TKey>
    {
        protected readonly IIdentityContext<TIdentity, TKey> _identityContext;

        public IdentifiedService(SimpleDbContext<TKey> context, IIdentityContext<TIdentity, TKey> identityContext) : base(context)
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
        public override async Task<TEntity?> GetById(TKey id, bool tracking = false)
        {
            var identity = await _identityContext.GetUserInfo();

            return await AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id) && x.OwnerId!.Equals(identity.Id));
        }
    }
}
