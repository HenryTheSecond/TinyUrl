using Microsoft.AspNetCore.Mvc;
using WriteTinyUrl.Interfaces.Services;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Controllers
{
    [ApiController]
    public class WriteUrlController(ITinyUrlService tinyUrlService) : ControllerBase
    {
        [HttpPost("createTinyUrl")]
        public async Task<string> CreateTinyUrl(WriteUrlRequest request)
        {
            return await tinyUrlService.CreateTinyUrlAsync(request.OriginalUrl);
        }
    }
}
