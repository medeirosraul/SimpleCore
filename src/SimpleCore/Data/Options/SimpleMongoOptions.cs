using MongoDB.Driver;
using SimpleCore.Entities;

namespace SimpleCore.Data.Options
{
    public class SimpleMongoOptions<TKey> 
    {
        public List<Func<SimpleMongoContext<TKey>, Task>> CreateIndexActions { get; set; } = new List<Func<SimpleMongoContext<TKey>, Task>>();

        public string? ConnectionString { get; set; }
        public string? Database { get; set; }

        public void CreateIndex<T>(IndexKeysDefinition<T> keysDefinition) where T : Entity<TKey>
        {
            // Add function to list.
            async Task create(SimpleMongoContext<TKey> db) => await db.GetCollection<T>().Indexes.CreateOneAsync(new CreateIndexModel<T>(keysDefinition));
            CreateIndexActions.Add(create);
        }
    }
}
