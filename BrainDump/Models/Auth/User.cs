using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainDump.Models.Auth {
    public class User {
        public ObjectId Id { get; set; }

        [BsonElement("UserName")]
        [BsonRequired]
        public string UserName { get; set; }

        [BsonElement("UserId")]
        [BsonRequired]
        public long UserId { get; set; }

        [BsonElement("Salt")]
        [BsonRequired]
        public string Salt { get; set; }

        [BsonElement("Hash")]
        [BsonRequired]
        public string Hash { get; set; }

        [BsonConstructor]
        public User(
            ObjectId id,
            string username,
            long userid,
            string salt,
            string hash) {
            Id = id;
            UserName = username;
            UserId = userid;
            Salt = salt;
            Hash = hash;
        }

        public User(
            string username,
            long userid,
            string salt,
            string hash) {
            UserName = username;
            UserId = userid;
            Salt = salt;
            Hash = hash;
        }
    }
}
