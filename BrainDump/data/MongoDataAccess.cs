using BrainDump.data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BrainDump.Models {
    public class MongoDataAccess {
        private IMongoDatabase _database;

        public MongoDataAccess(IConfiguration configuration) {
            var connectionString = configuration["mongoConnectionString"];
            var dbName = configuration["mongoDatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);

            _database.GetCollection<BlogPost>("posts").Indexes.CreateOne(
                Builders<BlogPost>.IndexKeys.Ascending(_ => _.PostId),
                new CreateIndexOptions<BlogPost>() {Unique = true});
        }

        public IPostsRepository GetPostsRepository() {
            return new PostsRepository(_database.GetCollection<BlogPost>("posts"));
        }
    }
}