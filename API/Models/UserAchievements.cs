using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("UserAchievements", Schema = "public")]
    public class UserAchievements
    {
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("achievement_id")]
        public Guid AchievementId { get; set; }
    }
}
