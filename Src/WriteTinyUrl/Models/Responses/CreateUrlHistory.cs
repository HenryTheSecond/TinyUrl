namespace WriteTinyUrl.Models.Responses;

public class CreateUrlHistory
{
    public string Id { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTimeOffset Expire { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public long VisitedTime { get; set; }
}
