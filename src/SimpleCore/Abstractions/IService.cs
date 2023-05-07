using SimpleCore.Entities;
using SimpleCore.Types;

namespace SimpleCore.Abstractions
{
    /// <summary>
    /// An interface for base service with basic base entity manipulation.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IService<TEntity, TKey> 
        where TEntity : Entity<TKey>
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
        Task<TEntity?> GetById(TKey id, bool tracking = false);

        /// <summary>
        /// Get one entity by identification.
        /// </summary>
        /// <param name="id">Identification as string</param>
        /// <param name="tracking">Track entity?</param>
        /// <returns>Returns the entity that corresponds to passed identification.</returns>
        Task<TEntity?> GetById(string Id, bool tracking = false);

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
}