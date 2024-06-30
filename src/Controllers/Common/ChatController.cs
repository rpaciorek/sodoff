using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;

namespace sodoff.Controllers.Common
{
    public class ChatController : Controller
    {
        private readonly DBContext ctx;
        public ChatController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpPost]
        [Produces("application/xml")]
        [Route("ChatWebService.asmx/ReportUser")]
        [VikingSession]
        public IActionResult ReportUser(Viking viking, [FromForm] Guid reportUserID, [FromForm] int reportReason)
        {
            // TODO - Figure out what to do with report.

            Viking? reportedUser = ctx.Vikings.FirstOrDefault(e => e.Uid == reportUserID);
            if (reportedUser == null) return Ok(false);
            ReportReason reason = (ReportReason)reportReason;

            Console.WriteLine($"User {viking.Name} Reported {reportedUser.Name} For {reason}");

            return Ok(true);
        }
    }
}
