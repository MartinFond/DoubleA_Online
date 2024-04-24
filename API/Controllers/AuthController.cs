using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;
using System.Threading.Tasks;
using System.Text;
using NuGet.Protocol;


namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly JwtService _jwtService;

        public AuthController(IAuthenticationService authenticationService, JwtService jwtService)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _authenticationService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest("Invalid username or password");

            // Here you can generate a token or set up the user session as needed
            // For simplicity, let's just return the authenticated user
            
            string token = _jwtService.GenerateToken(user.Username, "user", user.Id.ToString());
            return Ok(new { Token = token });
            //return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {


            byte[] salt = _authenticationService.GenerateSalt();
            byte[] passwordHash = _authenticationService.CreatePasswordHash(model.Password, salt);

            var user = new User
            {
                Username = model.Username,
                RoleId = model.RoleId, // Assuming you'll validate and set this properly
                Email = "user@example.com",
                Salt = Convert.ToBase64String(salt),
                Password = Convert.ToBase64String(passwordHash),
            };

            var success = await _authenticationService.Register(user, passwordHash, salt);

            if (!success)
                return BadRequest("Username already exists");

            return Ok("Registration successful");
        }



        public class LoginRequest
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
        }

        public class RegisterRequest
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
            public int RoleId { get; set; }
        }
    }
}

