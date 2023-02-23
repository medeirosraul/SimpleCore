using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SimpleCore.Abstractions;
using SimpleCore.Data;
using SimpleCore.Entities;

namespace SimpleCore.Services
{
    public class MongoService<TDocument, TKey> : IService<TDocument, TKey>
        where TDocument : Entity<TKey>
    {

        protected readonly SimpleMongoContext<TKey> Context;

        public MongoService(SimpleMongoContext<TKey> context)
        {
            Context = context;
        }

        protected virtual IQueryable<TDocument> AsQueryable(bool tracking = false)
        {
            return Context.GetCollection<TDocument>().AsQueryable();
        }

        public Task<TDocument?> GetById(TKey id, bool tracking = false)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return AsQueryable().FirstOrDefaultAsync(p => p.Id!.Equals(id) && !p.Deleted);
        }

        public Task Insert(TDocument entity)
        {
            return Context.GetCollection<TDocument>().InsertOneAsync(entity);
        }
    }
}
