using Microsoft.EntityFrameworkCore;
using API.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievements> UserAchievements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<RoleType>();
            modelBuilder.HasPostgresEnum<RankType>();
            modelBuilder.Entity<UserAchievements>()
                .HasKey(ua => new { ua.UserId, ua.AchievementId });

                base.OnModelCreating(modelBuilder); 
        }
    }
}
