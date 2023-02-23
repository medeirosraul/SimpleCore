using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleCore.Data.Options;
using MongoDB.Driver.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;
using SimpleCore.Entities;

namespace SimpleCore.Data
{
    public class SimpleMongoContext<TKey>
    {
        private static bool _firstRun = true;
        private readonly SimpleMongoOptions<TKey> _options;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public SimpleMongoContext(IOptionsMonitor<SimpleMongoOptions<TKey>> optionsAccessor)
        {
            _options = optionsAccessor.CurrentValue;

            // Register type especific serialization
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            // Mongo connection config
            var url = new MongoUrl(_options.ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);

            // Configure debug
            settings.ClusterConfigurator = cb => {
                cb.Subscribe<CommandStartedEvent>(e => {
                    Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            // Other settings
            settings.RetryWrites = false;

            // Client creation
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(_options.Database);

            // First run
            if (_firstRun)
            {
                CreateIndexes().Wait();
                _firstRun = false;
            }
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name) where TDocument : Entity<TKey>
        {
            return _database.GetCollection<TDocument>(name);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : Entity<TKey>
        {
            // Try get name.
            var name = typeof(TDocument).GetCustomAttribute<BsonDiscriminatorAttribute>()?.Discriminator;

            // If does not have name, get class name.
            if (string.IsNullOrWhiteSpace(name))
                name = typeof(TDocument).Name;

            return GetCollection<TDocument>(name);
        }

        private async Task CreateIndexes()
        {
            foreach (var action in _options.CreateIndexActions)
            {
                if (action != null)
                    await action.Invoke(this);
            }
        }
    }
}
