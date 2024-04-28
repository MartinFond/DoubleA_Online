
using Microsoft.AspNetCore.Mvc;
using API.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers{

    [ApiController]
    [Route("api/sessions")]
    public class SessionController : ControllerBase
    {
        private readonly RedisService _redisService;

        public SessionController(RedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost]
        [Authorize(Roles = "DGS")]
        public async Task<IActionResult> RegisterSession([FromBody] Session session)
        {
            if (string.IsNullOrEmpty(session.sessionId) || session == null)
            {
                return BadRequest("Invalid session data");
            }

            var success = await _redisService.RegisterSessionAsync(session.sessionId, session);
            if (success)
            {
                return Ok("Session registered successfully");
            }
            else
            {
                return StatusCode(500, "Failed to register session");
            }
        }

        [HttpGet("{sessionId}")]
        [Authorize(Roles = "DGS")]
        public async Task<IActionResult> GetSession(string sessionId)
        {
            var session = await _redisService.GetSessionAsync(sessionId);
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }
    }
}