using HR.LeaveManagement.MVC.Models.LeaveAllocation;

namespace HR.LeaveManagement.MVC.Models.LeaveRequest;

public class EmployeeLeaveRequestViewVM
{
    public List<LeaveAllocationVM> LeaveAllocations { get; set; }
    public List<LeaveRequestVM> LeaveRequests { get; set; }
}
