using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SimpleCore.Abstractions;
using SimpleCore.Entities;

namespace SimpleCore.Mongo
{
    public interface IMongoRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {

    }

    public class MongoRepository<TEntity, TKey> : IMongoRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
        //where TQueryable : IMongoQueryable<TEntity>
    {
        private readonly MongoContext _context;
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(MongoContext context)
        {
            _context = context;
            _collection = _context.Collection<TEntity>();
        }

        public IMongoQueryable<TEntity> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public Task Insert(TEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }
    }
}
