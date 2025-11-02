using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HR.LeaveManagement.Identity;

public class LeaveManagementIdentityDbContextFactory
       : IDesignTimeDbContextFactory<LeaveManagementIdentityDbContext>
{
    public LeaveManagementIdentityDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("LeaveManagementIdentityConnectionString");

        var builder = new DbContextOptionsBuilder<LeaveManagementIdentityDbContext>();
        builder.UseSqlServer(connectionString);

        return new LeaveManagementIdentityDbContext(builder.Options);
    }
}