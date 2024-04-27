using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using API.Data;
using API.Services;

namespace API.Controllers
{
    [Authorize] // Requires authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AchievementService _achievementService;

        public AchievementsController(ApplicationDbContext context, AchievementService achievementService)
        {
            _context = context;
            _achievementService = achievementService;
        }

        [HttpGet]
        public IActionResult GetUserAchievements()
        {
            // Retrieve authenticated user's ID from JWT token
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Log the userIdString
            Console.WriteLine($"User ID from JWT token: {userIdString}");

            // Convert user ID string to Guid
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("Invalid user ID format");
            }

            // Query UserAchievements table to retrieve achievements linked to the user's ID
            var userAchievements = _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.AchievementId)
                .ToList();

            return Ok(userAchievements);
        }

        [HttpPost("grant")]
        [Authorize(Roles = "2")] // Ensure only DGS users can access this endpoint
        public async Task<IActionResult> GrantAchievement([FromBody] GrantAchievementRequest request)
        {
            // Validate request parameters
            if (request.UserId == Guid.Empty || request.AchievementId == Guid.Empty)
            {
                return BadRequest("Invalid request parameters");
            }

            // Call the service to grant the achievement
            var result = await _achievementService.GrantAchievement(request.UserId, request.AchievementId);

            if (result.IsSuccess)
            {
                return Ok("Achievement granted successfully");
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
