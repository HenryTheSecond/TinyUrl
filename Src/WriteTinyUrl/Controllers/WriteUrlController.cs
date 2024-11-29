using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WriteTinyUrl.Interfaces.Services;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Controllers
{
    [ApiController, Authorize]
    public class WriteUrlController(ITinyUrlService tinyUrlService) : ControllerBase
    {
        [HttpPost("createTinyUrl")]
        public async Task<string> CreateTinyUrl(WriteUrlRequest request)
        {
            return await tinyUrlService.CreateTinyUrlAsync(new UserInfo(HttpContext.User), request.OriginalUrl);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetCreateUrlHistories(DateTimeOffset? lastCreatedDateTime, string? lastId, int take = 5)
        {
            if ((lastCreatedDateTime != null && lastId == null) ||
                (lastCreatedDateTime == null && lastId != null))
            {
                return BadRequest($"{nameof(lastCreatedDateTime)} and {nameof(lastId)} must be either both equal or not equal null");
            }

            var userInfo = new UserInfo(HttpContext.User);
            return Ok(await tinyUrlService.GetCreateUrlHistories(userInfo.Sub, take, lastCreatedDateTime, lastId));
        }
    }
}
