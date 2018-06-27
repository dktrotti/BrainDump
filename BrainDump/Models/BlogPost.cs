using System;
using System.Security.Cryptography;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace BrainDump.Models {
    public class BlogPost {
        [JsonIgnore]
        public ObjectId Id { get; set; }

        [BsonElement("PostId")]
        [BsonRequired]
        public long PostId { get; set; }

        [BsonElement("CreationTime")]
        [BsonRequired]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreationTime { get; set; }

        [BsonElement("Title")]
        [BsonRequired]
        public string Title { get; set; }

        [BsonElement("Body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("UserId")]
        public long? UserId { get; set; }

        [BsonConstructor]
        public BlogPost(
                ObjectId id,
                long postId,
                DateTime creationTime,
                string title,
                string body,
                long? userId) {
            Id = id;
            PostId = postId;
            CreationTime = creationTime;
            Title = title;
            Body = body;
            UserId = userId;
        }

        public BlogPost(long postId, long userId, BlogPostSubmission submission) {
            PostId = postId;
            CreationTime = DateTime.UtcNow;
            Title = submission.Title;
            Body = submission.Body;
            UserId = userId;
        }
    }
}