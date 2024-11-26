namespace UserGetUrlConsumer.Models;

public class UserVisitMessage
{
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string TinyUrl { get; set; } = string.Empty;
    public string? OriginalUrl { get; set; }
}
