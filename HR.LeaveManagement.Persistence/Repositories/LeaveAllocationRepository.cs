using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    private readonly LeaveManagementDbContext _dbContext;

    public LeaveAllocationRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAllocations(List<LeaveAllocation> allocations) =>
        await _dbContext.AddRangeAsync(allocations);

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period) =>
        await _dbContext.LeaveAllocations
            .AnyAsync(q => q.EmployeeId == userId
                                    && q.LeaveTypeId == leaveTypeId
                                    && q.Period == period);

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails() =>
        await _dbContext.LeaveAllocations
           .Include(q => q.LeaveType)
           .ToListAsync();


    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId) =>
        await _dbContext.LeaveAllocations
            .Where(q => q.EmployeeId == userId)
           .Include(q => q.LeaveType)
           .ToListAsync();

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id) =>
        await _dbContext.LeaveAllocations
            .Include(q => q.LeaveType)
            .FirstOrDefaultAsync(q => q.Id == id);

    public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId) =>
        await _dbContext.LeaveAllocations
            .FirstOrDefaultAsync(q => q.EmployeeId == userId
                                               && q.LeaveTypeId == leaveTypeId);
}