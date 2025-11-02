using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        // Admin User
        var admin = new ApplicationUser
        {
            Id = "400E0CE2-8405-4CB2-93AA-0BB4E59558B2",
            Email = "admin@local.com",
            NormalizedEmail = "ADMIN@LOCAL.COM",
            FirstName = "System",
            LastName = "Admin",
            UserName = "admin@local.com",
            NormalizedUserName = "ADMIN@LOCAL.COM",
            EmailConfirmed = true,
            SecurityStamp = "6E0465BE-8B27-4EC8-BB61-F75E47D527AB",
            ConcurrencyStamp = "74C91904-C748-4349-A6F9-6D0BF259AB6F"
        };
        admin.PasswordHash = hasher.HashPassword(admin, "1qaz!QAZ");

        // Regular User
        var user = new ApplicationUser
        {
            Id = "DC9BD49C-5DFF-4555-AFB9-550067551265",
            Email = "user@local.com",
            NormalizedEmail = "USER@LOCAL.COM",
            FirstName = "System",
            LastName = "User",
            UserName = "user@local.com",
            NormalizedUserName = "USER@LOCAL.COM",
            EmailConfirmed = true,
            SecurityStamp = "2A7FA37A-4078-432D-88CE-09CB2C63D7A6",
            ConcurrencyStamp = "2A7FA37A-4078-432D-88CE-09CB2C63D7A6"
        };
        user.PasswordHash = hasher.HashPassword(user, "1qaz!QAZ");

        builder.HasData(admin, user);
    }
}

