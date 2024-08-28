using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UrlGenerator
{
    [Route("api/test")]
    [ApiController]
    public class ValuesController(UrlRangeContext context) : ControllerBase
    {
        [HttpGet]
        public void Get()
        { 
        
        }
    }
}
