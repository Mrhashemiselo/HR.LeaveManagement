using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
public class CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<CreateLeaveTypeCommand, int>
{
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveTypeDtoValidator();
        var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

        if (!validationResult.IsValid)
            throw new Exception();

        var leaveType = mapper.Map<LeaveType>(request.LeaveTypeDto);
        leaveType = await leaveTypeRepository.Add(leaveType);
        return leaveType.Id;
    }
}
