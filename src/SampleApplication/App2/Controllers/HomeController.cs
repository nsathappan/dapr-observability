using Microsoft.AspNetCore.Mvc;

namespace App2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong from App2");
        }
    }
}
