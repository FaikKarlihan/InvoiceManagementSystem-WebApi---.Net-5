using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace PaymentApi.Context
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
    
        public MongoContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoConnection");
            var databaseName = configuration["DatabaseSettings:DatabaseName"];
    
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
    
        public IMongoDatabase Database => _database;
    }
}