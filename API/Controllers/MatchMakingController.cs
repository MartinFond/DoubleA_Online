using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using API.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchmakingController : ControllerBase
    {
        private readonly MatchmakingService _matchmakingService;
        private readonly IHubContext<MatchMakingHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public MatchmakingController(MatchmakingService matchmakingService, IHubContext<MatchMakingHub> hubContext, ApplicationDbContext context)
        {
            _matchmakingService = matchmakingService;
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost("player")]
        [Authorize(Roles = "Player")] // Ensure only authenticated users can access this endpoint
        public async Task<IActionResult> JoinMatchmaking()
        {
            Console.WriteLine("Player Joining");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userId;
            if (userIdClaim != null)
            {
                userId = userIdClaim.Value;
                // Now you have the user ID
            }
            else
            {
                // Handle the case where user ID claim is not found in the JWT token
                return BadRequest("User Id not found");
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == new Guid(userId));
            if (user == null)
            {
                return BadRequest("User not in DataBase");
            }

            var result = _matchmakingService.JoinMatchmaking(userId, user.Rank);
            if (result.IsSuccess)
            {
                return Ok("Joined matchmaking");
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpPost("player/update")]
        [Authorize(Roles = "Player")]
        public IActionResult PullUpdatePlayer()
        {
            Console.WriteLine("PullUpdatePlayer");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userId;
            if (userIdClaim != null)
            {
                userId = userIdClaim.Value;
                // Now you have the user ID
            }
            else
            {
                // Handle the case where user ID claim is not found in the JWT token
                return BadRequest("User Id not found");
            }
            string? serverAddress = _matchmakingService.PullUpdateUser(userId);
            return Ok(serverAddress);
        }

        [HttpPost("dgs")]
        [Authorize(Roles = "DGS")] // Ensure only Dedicated Game Servers can access this endpoint
        public IActionResult AddDGSToMatchmaking()
        {
            var ipAddressClaim = User.FindFirst("ip_address");
            string dgsAddress;
            if (ipAddressClaim != null)
            {
                dgsAddress = ipAddressClaim.Value;   
            }
            else
            {
                return BadRequest("Server ID not found");
            }
            if (dgsAddress == "not_needed")
            {
                return BadRequest("Invalid IP");
            }

            // Assuming you have the necessary data to pass as an argument, such as DGS address
            var result = _matchmakingService.AddDgsToMatchmaking(dgsAddress);
            if (result.IsSuccess)
            {
                return Ok("Added DGS to matchmaking");
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpPost("dgs/update")]
        [Authorize(Roles = "DGS")] // Ensure only Dedicated Game Servers can access this endpoint
        public IActionResult PullUpdateServer()
        {
            var ipAddressClaim = User.FindFirst("ip_address");
            string dgsAddress;
            if (ipAddressClaim != null)
            {
                dgsAddress = ipAddressClaim.Value;   
            }
            else
            {
                return BadRequest("Server ID not found");
            }
            if (dgsAddress == "not_needed")
            {
                return BadRequest("Invalid IP");
            }
            
            List<string>? users = _matchmakingService.PullUpdateServer(dgsAddress);
            if (users != null)
            {
                return Ok(users);
            }
            return Ok("MatchNotReady");
        }
    }
}
