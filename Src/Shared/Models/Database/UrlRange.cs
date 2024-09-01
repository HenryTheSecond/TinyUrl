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
