using HR.LeaveManagement.Application.DTOs.Common;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Models.Identity;
using System;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest;

public class LeaveRequestListDto : BaseDto
{
    public Employee Employee { get; set; }
    public string RequestingEmployeeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; }
    public DateTimeOffset DateRequested { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public bool? Approved { get; set; }
}