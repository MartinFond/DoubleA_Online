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
            Console.WriteLine("Login begin");
            var user = await _authenticationService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest("Invalid username or password");

            // Here you can generate a token or set up the user session as needed
            // For simplicity, let's just return the authenticated user
            Console.WriteLine("Connection ok, generating token");
            string token = _jwtService.GenerateToken(user.Username, user.Role, user.Id.ToString());
            return Ok(new { Token = token });
            //return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {


            byte[] salt = _authenticationService.GenerateSalt();
            byte[] passwordHash = _authenticationService.CreatePasswordHash(model.Password, salt);

            Enum.TryParse<RoleType>(model.Role, true, out RoleType role);
            var user = new User
            {
                Username = model.Username,
                Role = role, // Assuming you'll validate and set this properly
                Email = model.Email,
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
            public required string Email { get; set; }
            public required string Role { get; set; }
        }
    }
}

