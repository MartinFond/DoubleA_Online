using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using API.Data;

namespace API.Controllers
{
    [Authorize] // Requires authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AchievementsController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
