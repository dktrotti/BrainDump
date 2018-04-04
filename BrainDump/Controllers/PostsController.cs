using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrainDump.Controllers {
    [Route("api/[controller]")]
    public class PostsController : Controller {
        private readonly BrainDumpContext _context;

        public PostsController(BrainDumpContext context) {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<BlogPost> Get() {
            // TODO: Remove this method
            return _context.BlogPosts;
        }

        [HttpGet("{id}", Name = "GetBlogPost")]
        public IActionResult Get(long id) {
            var blogPost = _context.BlogPosts.Find(id);
            if (blogPost == null) {
                return NotFound();
            } else {
                return new ObjectResult(blogPost);
            }
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] BlogPostSubmission submission) {
            if (submission == null) {
                return BadRequest();
            }

            var blogPost = new BlogPost(submission);

            _context.BlogPosts.Add(blogPost);
            _context.SaveChanges();

            return CreatedAtRoute("GetBlogPost", new { id = blogPost.Id }, blogPost);
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
