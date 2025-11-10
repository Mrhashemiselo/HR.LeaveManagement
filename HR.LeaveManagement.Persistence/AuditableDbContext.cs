using HR.LeaveManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence;
public class AuditableDbContext : DbContext
{
    public AuditableDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual async Task<int> SaveChangesAsync(string username = "SYSTEM")
    {
        var changeTracker = base.ChangeTracker
            .Entries<BaseDomainEntity>()
            .Where(q => q.State == EntityState.Added ||
                                               q.State == EntityState.Modified);

        foreach (var entry in changeTracker)
        {
            entry.Entity.LastModifiedDate = DateTime.Now;
            entry.Entity.LastModifiedBy = username;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                entry.Entity.CreatedBy = username;
            }
        }

        var result = await base.SaveChangesAsync();

        return result;
    }
}
