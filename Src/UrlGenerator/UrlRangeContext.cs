using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models.Database;
using System.Text;

namespace UrlGenerator;

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

        //InitializeUrlRangeRecursive(0, new StringBuilder());

        void InitializeUrlRangeRecursive(int length, StringBuilder sb)
        {
            if (length == DefaultNumberOfUrlCharacter)
            {
                builder.HasData(new UrlRange
                {
                    Id = Ulid.NewUlid(),
                    Url = sb.ToString(),
                    IsUsed = false
                });
                return;
            }

            foreach (var ch in UrlAcceptedCharacters)
            {
                sb.Append(ch);
                InitializeUrlRangeRecursive(length + 1, sb);
                sb.Remove(length, 1);
            }
        }
    }
}
