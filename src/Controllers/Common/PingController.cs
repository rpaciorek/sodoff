using Microsoft.AspNetCore.Mvc;

namespace sodoff.Controllers.Common
{
    public class PingController : Controller
    {
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
