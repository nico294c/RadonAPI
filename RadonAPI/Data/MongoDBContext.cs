using MongoDB.Driver;

namespace RadonAPI.Data
{
    using RadonAPI.Models;
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Log> Logs => _database.GetCollection<Log>("Logs");
    }
}
