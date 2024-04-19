using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _authenticationService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest("Invalid username or password");

            // Here you can generate a token or set up the user session as needed
            // For simplicity, let's just return the authenticated user
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var user = new User
            {
                Username = model.Username,
                RoleId = model.RoleId // Assuming you'll validate and set this properly
            };

            byte[] salt = GenerateSalt();
            byte[] passwordHash = CreatePasswordHash(model.Password, salt);

            var success = await _authenticationService.Register(user, passwordHash, salt);

            if (!success)
                return BadRequest("Username already exists");

            return Ok("Registration successful");
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] CreatePasswordHash(string password, byte[] salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt bytes
                System.Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
                System.Buffer.BlockCopy(salt, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);

                return sha256.ComputeHash(saltedPasswordBytes);
            }
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public int RoleId { get; set; }
        }
    }
}

