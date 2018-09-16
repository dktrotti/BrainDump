using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainDump.Models.Auth {
    public class AccessToken {
        public ObjectId Id { get; set; }

        [BsonElement("TokenId")]
        [BsonRequired]
        public long TokenId { get; set; }

        [BsonElement("UserId")]
        [BsonRequired]
        public long UserId { get; set; }
    }
}
