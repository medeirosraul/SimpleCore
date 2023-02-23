using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleCore.Mongo
{
    /// <summary>
    /// Mongo Database Context
    /// </summary>
    public class MongoContext
    {
        private static bool _firstRun = true;

        private readonly MongoOptions _options;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Create new instance of Mongo Database Context
        /// </summary>
        /// <param name="optionsAccessor"></param>
        public MongoContext(IOptionsMonitor<MongoOptions> optionsAccessor)
        {
            // Register type especific serialization
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            // Context instance
            _options = optionsAccessor.CurrentValue;
            var mongoConnectionUrl = new MongoUrl(_options.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            // Register command logging
            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    Debug.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            // Ssl settings
            //mongoClientSettings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            // Other settings
            mongoClientSettings.RetryWrites = false;

            // Create client instance
            _client = new MongoClient(mongoClientSettings);
            _database = _client.GetDatabase(_options.DatabaseName);

            if (_firstRun)
            {
                Task.Run(async () => await CreateIndexes());
                _firstRun = false;
            }
        }

        /// <summary>
        /// Get a Mongo Collection of type T
        /// </summary>
        /// <typeparam name="TDocument">Type of collection</typeparam>
        /// <param name="name">Name of collection</param>
        /// <returns>Collection of type T</returns>
        public IMongoCollection<TDocument> Collection<TDocument>(string name) where TDocument : class
        {
            return _database.GetCollection<TDocument>(name);
        }

        /// <summary>
        /// Get a Mongo Collection of type T
        /// </summary>
        /// <typeparam name="TDocument">Type of collection</typeparam>
        /// <returns>Collection of type T</returns>
        public IMongoCollection<TDocument> Collection<TDocument>() where TDocument : class
        {
            // Try get name.
            var name = typeof(TDocument).GetCustomAttribute<BsonDiscriminatorAttribute>()?.Discriminator;

            // If does not have name, get class name.
            if (string.IsNullOrWhiteSpace(name))
                name = typeof(TDocument).Name;

            return Collection<TDocument>(name);
        }

        /// <summary>
        /// Run database index creation
        /// </summary>
        private async Task CreateIndexes()
        {
            await _options.IndexCreation(this);
        }
    }
}
