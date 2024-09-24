using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;

namespace TinyUrlJobs
{
    public class UrlRangeContext: DbContext
    {
        public UrlRangeContext(DbContextOptions<UrlRangeContext> options) : base(options) { }

        public DbSet<UrlRange> UrlRanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UrlRangeConfiguration());
        }
    }
}
