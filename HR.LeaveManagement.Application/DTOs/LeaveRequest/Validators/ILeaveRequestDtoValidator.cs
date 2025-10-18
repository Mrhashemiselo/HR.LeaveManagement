using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class ILeaveRequestDtoValidator :AbstractValidator<ILeaveRequestDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public ILeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            
            RuleFor(r => r.StartDate)
                .LessThan(l => l.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");
            
            RuleFor(r => r.EndDate)
                .LessThan(l => l.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");
            
            RuleFor(r => r.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var leaveTypeExists = await _leaveTypeRepository.Exists(id);
                    return leaveTypeExists;
                }).WithMessage("{PropertyName} does not exists");
        }
    }
}