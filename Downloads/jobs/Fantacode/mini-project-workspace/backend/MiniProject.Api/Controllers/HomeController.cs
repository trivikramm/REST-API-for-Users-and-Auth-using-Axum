using Microsoft.AspNetCore.Mvc;

namespace MiniProject.Api.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("MiniProject API is running.");
        }
    }
}
