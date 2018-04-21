using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models;

namespace BrainDump.data {
    public interface IPostsRepository {
        void Add(BlogPost item);
        IEnumerable<BlogPost> GetAll();
        BlogPost Find(long key);
        void Remove(long key);
        void Update(BlogPost item);
    }
}
