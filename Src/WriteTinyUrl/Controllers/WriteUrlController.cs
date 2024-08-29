using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Controllers
{
    [ApiController]
    public class WriteUrlController(UrlRangeContext context) : ControllerBase
    {
        [HttpPost("createTinyUrl")]
        public async Task<string> CreateTinyUrl()
        {
            
            return "aaaa";
        }
    }
}
