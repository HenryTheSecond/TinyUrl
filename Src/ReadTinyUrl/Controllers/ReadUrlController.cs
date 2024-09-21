using Microsoft.AspNetCore.Mvc;
using ReadTinyUrl.Interfaces.Services;

namespace ReadTinyUrl.Controllers
{
    [ApiController]
    public class ReadUrlController(ITinyUrlService tinyUrlService) : ControllerBase
    {
        [HttpGet("{tinyUrl}")]
        public async Task<string> ReadTinyUrlAsync(string tinyUrl)
        {
            return await tinyUrlService.ReadUrlAsync(tinyUrl);
        }
    }
}
