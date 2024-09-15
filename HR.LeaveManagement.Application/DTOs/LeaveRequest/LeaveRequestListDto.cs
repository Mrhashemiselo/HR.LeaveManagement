using HR.LeaveManagement.Application.DTOs.Common;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest;
internal class LeaveRequestListDto : BaseDto
{
    public LeaveTypeDto LeaveType { get; set; }
    public DateTime DateRequested { get; set; }
    public bool? Approved { get; set; }
}
