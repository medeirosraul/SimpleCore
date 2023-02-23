using MongoDB.Driver;

namespace SimpleCore.Mongo
{
    public class MongoOptions
    {
        private ICollection<Func<MongoContext, Task>>? _buildActions;
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }

        public void CreateIndex<T>(IndexKeysDefinition<T> keysDefinition) where T : class
        {
            // Ensure that buildActions is not null.
            _buildActions ??= new List<Func<MongoContext, Task>>();

            // Add function to list.
            Func<MongoContext, Task> func = async (db) => await db.Collection<T>().Indexes.CreateOneAsync(new CreateIndexModel<T>(keysDefinition));
            _buildActions.Add(func);
        }

        public async Task IndexCreation(MongoContext db)
        {
            if (_buildActions != null)
            {
                foreach (var func in _buildActions)
                {
                    if (func == null)
                        continue;

                    await func.Invoke(db);
                }
            }
        }
    }
}
