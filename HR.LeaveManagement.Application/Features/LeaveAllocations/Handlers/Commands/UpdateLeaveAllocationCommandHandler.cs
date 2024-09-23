using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands;
public class UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var leaveAllocation = await leaveAllocationRepository.Get(request.LeaveAllocationDto.Id);
        mapper.Map(request.LeaveAllocationDto, leaveAllocation);
        await leaveAllocationRepository.Update(leaveAllocation);
        return Unit.Value;
    }
}
