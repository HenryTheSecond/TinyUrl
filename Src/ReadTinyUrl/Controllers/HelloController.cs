using Microsoft.AspNetCore.Mvc;

namespace ReadTinyUrl.Controllers
{
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet("hello")]
        public string Hello()
        {
            return "Hello";
        }
    }
}
