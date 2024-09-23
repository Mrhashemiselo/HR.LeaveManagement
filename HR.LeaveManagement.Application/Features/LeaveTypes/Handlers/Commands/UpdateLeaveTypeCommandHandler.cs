using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
public class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<UpdateLeaveTypeCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var leaveType = await leaveTypeRepository.Get(request.LeaveTypeDto.Id);
        mapper.Map(request.LeaveTypeDto, leaveType);
        await leaveTypeRepository.Update(leaveType);
        return Unit.Value;
    }
}
