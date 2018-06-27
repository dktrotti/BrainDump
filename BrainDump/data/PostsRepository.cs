using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models;
using MongoDB.Driver;

namespace BrainDump.data {
    public class PostsRepository : IPostsRepository {
        private readonly IMongoCollection<BlogPost> _posts;

        public PostsRepository(IMongoCollection<BlogPost> posts) {
            _posts = posts;
        }

        public void Add(BlogPost item) {
            _posts.InsertOne(item);
        }

        public IEnumerable<BlogPost> GetAll() {
            return _posts.Find(p => true).ToEnumerable();
        }

        public BlogPost Find(long key) {
            return _posts.Find(p => p.PostId == key).FirstOrDefault();
        }

        public void Remove(long key) {
            _posts.DeleteOne(p => p.PostId == key);
        }

        public void Update(BlogPost item) {
            throw new NotImplementedException();
        }
    }
}
