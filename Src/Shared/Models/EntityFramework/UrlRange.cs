using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.Database;

[Table(TableName)]
public class UrlRange : IIdentifier
{
    public const string TableName = "UrlRange";
    public Ulid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
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
