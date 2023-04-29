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
        /// Get one entity by identification.
        /// </summary>
        /// <param name="id">Identification</param>
        /// <param name="tracking">Track entity?</param>
        /// <returns>Returns the entity that corresponds to passed identification.</returns>
        Task<TEntity?> GetById(TKey id, bool tracking = false);

        Task<PagedList<TEntity>> Get(bool tracking = false);

        Task<PagedList<TEntity>> Get(IQueryable<TEntity>? query, bool tracking = false);

        Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity>? query, bool tracking = false);
    }
}