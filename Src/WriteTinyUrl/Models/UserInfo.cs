using System.Security.Claims;

namespace WriteTinyUrl.Models;

public class UserInfo
{
    public string Sub { get; set; }
    public string Username { get; set; }
    public string? Email { get; set; }

    public UserInfo(ClaimsPrincipal claimsPrincipal)
    {
        var claims = claimsPrincipal.Claims;
        Sub = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        Username = claims.Single(x => x.Type == "preferred_username").Value;
        Email = claims.SingleOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    }
}
