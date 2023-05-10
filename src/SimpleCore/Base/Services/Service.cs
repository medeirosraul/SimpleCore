using Microsoft.EntityFrameworkCore;
using SimpleCore.Base.Entities;
using SimpleCore.Data;
using SimpleCore.Types;

namespace SimpleCore.Base.Services;


/// <summary>
/// An interface for base service with basic base entity manipulation.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IService<TEntity>
    where TEntity : Entity
{
    /// <summary>
    /// Insert one entity in database.
    /// </summary>
    /// <param name="entity">Entity to be inserted.</param>
    Task Insert(TEntity entity);

    /// <summary>
    /// Update all modified entities in context.
    /// </summary>
    Task UpdateAndSaveChanges();

    /// <summary>
    /// Update an entity in database.
    /// </summary>
    /// <param name="entity">Entity to be updated.</param>
    Task Update(TEntity entity);

    /// <summary>
    /// Get one entity by identification.
    /// </summary>
    /// <param name="id">Identification</param>
    /// <param name="tracking">Track entity?</param>
    /// <returns>Returns the entity that corresponds to passed identification.</returns>
    Task<TEntity?> GetById(string id, bool tracking = false);

    /// <summary>
    /// Get all entities.
    /// </summary>
    /// <param name="tracking">Track entities?</param>
    /// <returns></returns>
    Task<PagedList<TEntity>> Get(bool tracking = false);

    /// <summary>
    /// Get paged entities.
    /// </summary>
    /// <param name="page">Current page (init by 1).</param>
    /// <param name="limit">Limit itens per page.</param>
    /// <param name="tracking">Track entities?</param>
    /// <returns>A paged list of entities.</returns>
    Task<PagedList<TEntity>> Get(int page, int limit, bool tracking = false);

    /// <summary>
    /// Get paged entities with query.
    /// </summary>
    /// <param name="page">Current page (init by 1).</param>
    /// <param name="limit">Limit itens per page.</param>
    /// <param name="query">Query to filter entities.</param>
    /// <param name="tracking">Track entities?</param>
    /// <returns>A paged list of entities.</returns>
    Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity>? query, bool tracking = false);

    /// <summary>
    /// Get all entities with query.
    /// </summary>
    /// <param name="query">Query to filter entities.</param>
    /// <param name="tracking">Track entities?</param>
    /// <returns>A paged list of entities, with page fixed at 1 and no limit.</returns>
    Task<PagedList<TEntity>> Get(IQueryable<TEntity>? query, bool tracking = false);
}

public class Service<TEntity> : IService<TEntity>
    where TEntity : Entity
{
    protected readonly SimpleDbContext Context;

    public Service(SimpleDbContext context)
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

    public virtual Task<TEntity?> GetById(string id, bool tracking = false)
    {
        return AsQueryable(tracking).FirstOrDefaultAsync(x => x.Id!.Equals(id));
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