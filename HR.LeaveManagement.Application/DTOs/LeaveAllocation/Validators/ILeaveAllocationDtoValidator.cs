using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
public class ILeaveAllocationDtoValidator : AbstractValidator<LeaveAllocationDto>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;

    public ILeaveAllocationDtoValidator(ILeaveRequestRepository leaveRequestRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
        RuleFor(s => s.NumberOfDays)
            .GreaterThan(0).WithMessage("{PropertyName} must be before {ComparisonValue}");

        RuleFor(s => s.Period)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be before {ComparisonValue}");

        RuleFor(w => w.LeaveTypeId)
            .GreaterThan(0)
            .MustAsync(async (id, token) =>
            {
                var leaveTypeExists = await _leaveRequestRepository.Exists(id);
                return !leaveTypeExists;
            }).WithMessage("{PropertyName} does not exist");
    }
}
