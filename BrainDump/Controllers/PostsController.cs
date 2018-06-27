using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BrainDump.data;
using BrainDump.Models;
using BrainDump.Models.Auth;
using BrainDump.util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainDump.Controllers {
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PostsController : Controller {
        private readonly Random _random;
        private readonly IPostsRepository _posts;

        public PostsController(MongoDataAccess dataAccess, Random random) {
            _random = random;
            _posts = dataAccess.GetPostsRepository();
        }

        [HttpGet]
        public IEnumerable<BlogPost> Get() {
            // TODO: Remove this method
            return _posts.GetAll();
        }

        [HttpGet("{id}", Name = "GetBlogPost")]
        public IActionResult Get(long id) {
            var blogPost = _posts.Find(id);
            if (blogPost == null) {
                return NotFound();
            } else {
                return new ObjectResult(blogPost);
            }
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] BlogPostSubmission submission) {
            var userId = HttpContext.GetUserId();
            if (submission == null || userId == null) {
                return BadRequest();
            }

            var blogPost = new BlogPost(_random.NextPositiveLong(), userId.Value, submission);

            _posts.Add(blogPost);

            return CreatedAtRoute("GetBlogPost", new { id = blogPost.Id }, blogPost);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            var userId = HttpContext.GetUserId();
            if (userId == null) {
                return BadRequest();
            }
            var blogPost = _posts.Find(id);

            if (blogPost == null || userId == null || blogPost.UserId != userId) {
                return Forbid();
            }

            _posts.Remove(id);
            return Ok();
        }
    }
}
