using System.Security.Claims;

namespace ReadTinyUrl.Interfaces.Services;

public interface IAnalyticsService
{
    Task SaveAnalyticInfo(ClaimsPrincipal user, string tinyUrl, string? originalUrl);
}
