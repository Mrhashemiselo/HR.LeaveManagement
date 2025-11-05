using HR.LeaveManagement.MVC.Models.LeaveType;
using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.MVC.Models.LeaveRequest;

public class LeaveRequestVM : CreateLeaveRequestVM
{
    public int Id { get; set; }

    [Display(Name = "Date Requested")]
    public DateTime DateRequested { get; set; }

    [Display(Name = "Date Actioned")]
    public DateTime DateActioned { get; set; }

    [Display(Name = "Approval State")]
    public bool? Approved { get; set; }

    public bool Cancelled { get; set; }
    public LeaveTypeVM LeaveType { get; set; }
    public EmployeeVM Employee { get; set; }
}
