using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models;
using BrainDump.Models.Auth;

namespace BrainDump.data {
    public interface IUserRepository {
        void Add(User item);
        IEnumerable<User> GetAll();
        User Find(long key);
        User Find(string username);
        void Remove(long key);
        void Update(User item);
    }
}
