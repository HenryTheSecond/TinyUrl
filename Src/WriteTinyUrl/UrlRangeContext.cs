using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;

namespace WriteTinyUrl
{
    public class UrlRangeContext : DbContext
    {
        public UrlRangeContext(DbContextOptions<UrlRangeContext> options) : base(options) { }

        public DbSet<UrlRange> UrlRanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UrlRangeConfiguration());
        }
    }

    public class UrlRangeConfiguration : IEntityTypeConfiguration<UrlRange>
    {
        private const int DefaultNumberOfUrlCharacter = 2;
        private const string UrlAcceptedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public void Configure(EntityTypeBuilder<UrlRange> builder)
        {
            builder.Property(x => x.Id).HasColumnType("char(26)").HasConversion(x => x.ToString(), x => Ulid.Parse(x));
        }
    }
}
