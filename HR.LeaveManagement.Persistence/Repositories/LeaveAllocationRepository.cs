using System.Collections.Generic;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveAllocationRepository:GenericRepository<LeaveAllocation>,ILeaveAllocationRepository
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
    }
}