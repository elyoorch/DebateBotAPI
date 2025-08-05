using DebateBotAPI.Models;
using DebateBotAPI.web.Models;
using DebateBotAPI.web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DebateBotAPI.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebateController : ControllerBase
    {
        private readonly DebateService _debateService;
        public DebateController(DebateService debateService)
        {
            _debateService = debateService;
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> GetMessageResponse([FromBody] DebateRequest request)
        {
            var reply = await _debateService.GetBotReply(request);
            return Ok(reply);
        }
    }
}
