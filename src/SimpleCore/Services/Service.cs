using Microsoft.EntityFrameworkCore;
using SimpleCore.Abstractions;
using SimpleCore.Data;
using SimpleCore.Entities;

namespace SimpleCore.Services
{
    public class Service<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        protected readonly SimpleDbContext<TKey> Context;

        public Service(SimpleDbContext<TKey> context)
        {
            Context = context;
        }

        protected virtual IQueryable<TEntity> AsQueryable(bool tracking = false)
        {
            if (tracking)
                return Context.Set<TEntity>().AsQueryable();

            return Context.Set<TEntity>().AsNoTracking().AsQueryable();
        }

        public virtual Task Insert(TEntity entity)
        {
            // Stamp
            entity.CreatedAt = DateTime.Now;
            entity.ModifiedAt = DateTime.Now;

            // Insert
            Context.Add(entity);

            // Save
            return Context.SaveChangesAsync();
        }

        public virtual Task<TEntity?> GetById(TKey id, bool tracking = false)
        {
            return AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id));
        }
    }
}