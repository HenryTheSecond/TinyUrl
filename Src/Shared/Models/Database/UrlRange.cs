using Shared.Interfaces;

namespace Shared.Models.Database;

public class UrlRange : IIdentifier
{
    public Ulid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
}
