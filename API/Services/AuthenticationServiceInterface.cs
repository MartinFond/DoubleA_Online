using System.Threading.Tasks;
using API.Models;

namespace API.Services
{
    public interface IAuthenticationService
    {
        Task<User?> Authenticate(string username, string password);
        Task<bool> Register(User user, byte[] passwordHash, byte[] salt);
        byte[] GenerateSalt();
        byte[] CreatePasswordHash(string password, byte[] salt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
