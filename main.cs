using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string secretKey = "YourSecretKey"; // Secret key for JWT token

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // Authenticate the user (validate username/password, retrieve role, etc.)
            // For simplicity, let's assume authentication is successful and retrieve the role.
            string userId = "123"; // Example user ID
            string role = "Player"; // Example role

            // Generate JWT token
            var token = GenerateToken(userId, role);

            // Return token
            return Ok(new { token });
        }

        private string GenerateToken(string userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", userId),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet("secure")]
        [Authorize(Roles = "Player")]
        public IActionResult SecureEndpoint()
        {
            // Only users with the role "Player" can access this endpoint
            return Ok("Secure endpoint accessed by Player role.");
        }

        [HttpGet("admin")]
        [Authorize(Roles = "DGS")]
        public IActionResult AdminEndpoint()
        {
            // Only users with the role "DGS" can access this endpoint
            return Ok("Admin endpoint accessed by DGS role.");
        }
    }
}
