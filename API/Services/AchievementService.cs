using API.Data;
using API.Models;

namespace API.Services{
    public class AchievementService
    {
        private readonly ApplicationDbContext _context;

        public AchievementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> GrantAchievement(Guid userId, Guid achievementId)
        {
            try
            {
                // Check if the user and achievement exist
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return ServiceResult.CreateError("User not found");
                }

                var achievement = await _context.Achievements.FindAsync(achievementId);
                if (achievement == null)
                {
                    return ServiceResult.CreateError("Achievement not found");
                }

                // Grant the achievement to the user
                var userAchievement = new UserAchievements
                {
                    UserId = userId,
                    AchievementId = achievementId
                };

                _context.UserAchievements.Add(userAchievement);
                await _context.SaveChangesAsync();

                return ServiceResult.CreateSuccess();
            }
            catch (Exception ex)
            {
                return ServiceResult.CreateError($"An error occurred: {ex.Message}");
            }
        }
    }

    public class GrantAchievementRequest
    {
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
    }
}