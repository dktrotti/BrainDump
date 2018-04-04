using System;

namespace BrainDump.Models {
    public class BlogPost {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long? UserId { get; set; }

        public BlogPost() {}

        public BlogPost(BlogPostSubmission submission) {
            CreationTime = DateTime.Now;
            Title = submission.Title;
            Body = submission.Body;
            UserId = null;
        }
    }
}