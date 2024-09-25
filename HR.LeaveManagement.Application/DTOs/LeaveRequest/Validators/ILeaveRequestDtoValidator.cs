using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
public class ILeaveRequestDtoValidator : AbstractValidator<ILeaveRequestDto>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public ILeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;

        RuleFor(p => p.StartDate)
            .LessThan(a => a.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");

        RuleFor(a => a.EndDate)
                    .GreaterThan(a => a.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

        RuleFor(s => s.LeaveTypeId)
            .GreaterThan(0)
            .MustAsync(async (id, token) =>
            {
                var leaveTypeExists = await _leaveTypeRepository.Exists(id);
                return !leaveTypeExists;
            }).WithMessage("{PropertyName} does not exist}");
    }
}
