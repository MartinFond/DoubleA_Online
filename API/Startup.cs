using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Npgsql;
using API.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Configuration.GetConnectionString("DefaultConnection"));
            dataSourceBuilder.MapEnum<RoleType>();
            dataSourceBuilder.MapEnum<RankType>();
            var dataSource = dataSourceBuilder.Build();
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(dataSource));
            services.AddScoped<IAuthenticationService, AuthenticationService>();

                    // Add JWT authentication
            var secretKey = Configuration["Jwt:SecretKey"];
            var issuer = Configuration["Jwt:Issuer"];

            if (secretKey == null || issuer == null)
            {
                Console.WriteLine("Bad configuration, check Issuer and SecretKey (can't be null)");
                System.Environment.Exit(0);
                return;
            }
            services.AddSingleton(new JwtService(secretKey, issuer));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            services.AddSignalR();
            services.AddControllers();
            services.AddScoped<AchievementService>();
            services.AddSingleton<MatchmakingService>();

            services.AddScoped<RedisService>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MatchMakingHub>("/matchmakinghub");
            });
        }
    }
}