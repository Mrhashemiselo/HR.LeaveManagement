using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    private readonly LeaveManagementDbContext _dbContext;

    public LeaveRequestRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
    {
        return await _dbContext.LeaveRequests
            .Include(i => i.LeaveType)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
    {
        return await _dbContext.LeaveRequests
            .Include(i => i.LeaveType)
            .ToListAsync();
    }

    public async Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? approvalStatus)
    {
        leaveRequest.Approved = approvalStatus;
        _dbContext.Entry(leaveRequest).State = EntityState.Modified;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
    {
        var leaveRequests = await _dbContext.LeaveRequests
            .Where(w => w.RequestingEmployeeId == userId)
            .Include(i => i.LeaveType)
            .ToListAsync();
        return leaveRequests;
    }
}