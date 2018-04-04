using Microsoft.EntityFrameworkCore;

namespace BrainDump.Models {
    public class BrainDumpContext : DbContext {
        public BrainDumpContext(DbContextOptions<BrainDumpContext> options) : base(options) {
            
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}