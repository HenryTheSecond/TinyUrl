using Microsoft.AspNetCore.Mvc;
using ReadTinyUrl.Interfaces.Services;

namespace ReadTinyUrl.Controllers
{
    [ApiController]
    public class ReadUrlController(ITinyUrlService tinyUrlService, IAnalyticsService analyticsService) : ControllerBase
    {
        [HttpGet("{tinyUrl}")]
        public async Task<string> ReadTinyUrlAsync(string tinyUrl)
        {
            string? originalUrl = null;
            try
            {
                originalUrl = await tinyUrlService.ReadUrlAsync(tinyUrl);
                return originalUrl;
            }
            finally
            {
                await analyticsService.SaveAnalyticInfo(HttpContext.User, tinyUrl, originalUrl);
            }
        }
    }
}
