using Microsoft.EntityFrameworkCore;
using SimpleCore.Abstractions;
using SimpleCore.Data;
using SimpleCore.Entities;
using SimpleCore.Types;

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

        protected virtual IQueryable<TEntity> PrepareQuery(bool tracking = false, bool deleted = false)
        {
            var query = AsQueryable(tracking);

            if (!deleted)
                query = query.Where(p => !p.Deleted);

            return query;
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

        public Task UpdateAndSaveChanges()
        {
            var entries = Context.ChangeTracker.Entries<TEntity>().Where(x => x.State == EntityState.Modified).ToList();

            if (entries.Any())
            {
                foreach (var entry in entries)
                {
                    var entity = entry.Entity;
                    entity.ModifiedAt = DateTime.Now;
                }
            }

            return Context.SaveChangesAsync();
        }

        public Task Update(TEntity entity)
        {
            // Stamp
            entity.ModifiedAt = DateTime.Now;

            // Update
            Context.Update(entity);

            // Save
            return Context.SaveChangesAsync();
        }

        public virtual Task<TEntity?> GetById(TKey id, bool tracking = false)
        {
            return AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id));
        }

        public virtual Task<TEntity?> GetById(string id, bool tracking = false)
        {

            if (typeof(TKey) == typeof(Guid))
            {
                return AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(Guid.Parse(id)));
            } 
            else if (typeof(TKey) == typeof(string))
            {
                return AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id));
            }
            else
            {
                throw new Exception("Invalid ID type.");
            }
        }

        public virtual Task<PagedList<TEntity>> Get(bool tracking = false)
        {
            return Get(null, false);
        }

        public virtual Task<PagedList<TEntity>> Get(int page, int limit, bool tracking = false)
        {
            return Get(page, limit, null, tracking);
        }

        public virtual async Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity>? query, bool tracking = false)
        {
            // Prepare query
            query ??= PrepareQuery(false, tracking);

            // Count
            var count = await query.CountAsync();

            // Create result
            page = page == 0 ? 1 : page;

            var result = new PagedList<TEntity>
            {
                TotalCount = count,
                PageIndex = page,
                PageSize = limit
            };

            // Return if count = 0
            if (count == 0) return result;

            // Paginate
            if (limit > 0)
                query = query.Skip((page - 1) * limit).Take(limit);

            // Return result
            result.AddRange(await query.ToListAsync());

            return result;
        }

        public virtual Task<PagedList<TEntity>> Get(IQueryable<TEntity>? query, bool tracking = false)
        {
            query ??= PrepareQuery(false, tracking);

            return Get(0, 0, query, tracking);
        }

    }
}