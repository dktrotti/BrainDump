using BrainDump.data;
using BrainDump.Models.Auth;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BrainDump.Models {
    public class MongoDataAccess {
        private readonly IMongoDatabase _mainDatabase;
        private readonly IMongoDatabase _authDatabase;

        public MongoDataAccess(IConfiguration configuration) {
            var mainConnectionString = configuration["mainConnectionString"];
            var mainDbName = configuration["mainDatabaseName"];
            var authConnectionString = configuration["authConnectionString"];
            var authDbName = configuration["authDatabaseName"];

            var mainClient = new MongoClient(mainConnectionString);
            _mainDatabase = mainClient.GetDatabase(mainDbName);
            var authClient = new MongoClient(authConnectionString);
            _authDatabase = authClient.GetDatabase(authDbName);

            _mainDatabase.GetCollection<BlogPost>("posts").Indexes.CreateOne(
                Builders<BlogPost>.IndexKeys.Ascending(_ => _.PostId),
                new CreateIndexOptions<BlogPost>() {Unique = true});

            _authDatabase.GetCollection<User>("users").Indexes.CreateOne(
                Builders<User>.IndexKeys.Ascending(_ => _.UserId),
                new CreateIndexOptions<User>() { Unique = true });
            _authDatabase.GetCollection<User>("users").Indexes.CreateOne(
                Builders<User>.IndexKeys.Ascending(_ => _.UserName),
                new CreateIndexOptions<User>() { Unique = true });

            _authDatabase.GetCollection<AccessToken>("accessTokens").Indexes.CreateOne(
                Builders<AccessToken>.IndexKeys.Ascending(_ => _.TokenId),
                new CreateIndexOptions<AccessToken>() { Unique = true });
        }

        public IPostsRepository GetPostsRepository() {
            return new PostsRepository(_mainDatabase.GetCollection<BlogPost>("posts"));
        }

        public IUserRepository GetUserRepository() {
            return new UserRepository(_authDatabase.GetCollection<User>("users"));
        }

        public ITokenRepository GetTokenRepository() {
            return new TokenRepository(_authDatabase.GetCollection<AccessToken>("refreshTokens"));
        }
    }
}