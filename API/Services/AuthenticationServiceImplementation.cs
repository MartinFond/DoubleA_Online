using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Text;


namespace API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("User Not Found");
                return null; // User not found
            }

            // Verify password
            if (!VerifyPasswordHash(password, Convert.FromBase64String(user.Password), Convert.FromBase64String(user.Salt)))
                {
                    Console.WriteLine("WrongPassword");
                    return null; // Invalid password
                }

            // Authentication successful
            return user;
        }

        public async Task<bool> Register(User user, byte[] passwordHash, byte[] salt)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return false; // Username already exists

            user.Password = Convert.ToBase64String(passwordHash);
            user.Salt = Convert.ToBase64String(salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] CreatePasswordHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt bytes
                Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);
                byte[] hash = sha256.ComputeHash(saltedPasswordBytes);


                return hash;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] computedHash = CreatePasswordHash(password, storedSalt);
                for (int i = 0; i < storedHash.Length; i++)
                {
                    if (storedHash[i] != computedHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
