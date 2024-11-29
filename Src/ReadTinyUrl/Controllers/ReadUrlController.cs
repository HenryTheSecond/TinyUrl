using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadTinyUrl.Interfaces.Services;
using System.Security.Claims;

namespace ReadTinyUrl.Controllers
{
    [ApiController, Authorize]
    public class ReadUrlController(ITinyUrlService tinyUrlService, IAnalyticsService analyticsService) : ControllerBase
    {
        [HttpGet("{tinyUrl}"), AllowAnonymous]
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

        [HttpGet("history")]
        public async Task<IActionResult> GetVisitHistory(DateTimeOffset? lastVistedTimeParam, string? lastId, int take = 5)
        {
            if ((lastVistedTimeParam != null && lastId == null) ||
                (lastVistedTimeParam == null && lastId != null))
            {
                return BadRequest($"{nameof(lastVistedTimeParam)} and {nameof(lastId)} must be either both equal or not equal null");
            }

            var userId = HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(await tinyUrlService.GetHistory(userId, take, lastVistedTimeParam, lastId));
        }
    }
}
