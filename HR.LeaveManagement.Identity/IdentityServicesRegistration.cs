using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using HR.LeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HR.LeaveManagement.Identity;
public static class IdentityServicesRegistration
{
    public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddDbContext<LeaveManagementIdentityDbContext>(options =>
        options.UseSqlServer(
            configuration.GetConnectionString("LeaveManagementIdentityConnectionString"),
            m => m.MigrationsAssembly(typeof(LeaveManagementIdentityDbContext).Assembly.FullName)));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Password rules — optional, you can adjust as needed
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<LeaveManagementIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(a =>
            {
                a.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings?.Key ?? throw new InvalidOperationException("JWT Key not found")))
                };
            });

        return services;
    }
}
