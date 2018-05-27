using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models.Auth;
using MongoDB.Driver;

namespace BrainDump.data {
    public class UserRepository : IUserRepository {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoCollection<User> users) {
            _users = users;
        }

        public void Add(User item) {
            _users.InsertOne(item);
        }

        public IEnumerable<User> GetAll() {
            return _users.Find(u => true).ToEnumerable();
        }

        public User Find(long key) {
            return _users.Find(u => u.UserId == key).FirstOrDefault();
        }

        public User Find(string username) {
            return _users.Find(u => u.UserName == username).FirstOrDefault();
        }

        public void Remove(long key) {
            throw new NotImplementedException();
        }

        public void Update(User item) {
            throw new NotImplementedException();
        }
    }
}
