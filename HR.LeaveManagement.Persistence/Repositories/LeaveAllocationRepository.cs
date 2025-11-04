using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    private readonly LeaveManagementDbContext _dbContext;
    public LeaveAllocationRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
    {
        return await _dbContext.LeaveAllocations
            .Include(i => i.LeaveType)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        return await _dbContext.LeaveAllocations
            .Include(i => i.LeaveType)
            .ToListAsync();
    }

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
    {
        return await _dbContext.LeaveAllocations
            .AnyAsync(a => a.EmployeeId == userId &&
                                        a.LeaveTypeId == leaveTypeId &&
                                        a.Period == period);
    }

    public async Task AddAllocations(List<LeaveAllocation> allocations)
    {
        await _dbContext.LeaveAllocations.AddRangeAsync(allocations);
        await _dbContext.SaveChangesAsync();
    }
}