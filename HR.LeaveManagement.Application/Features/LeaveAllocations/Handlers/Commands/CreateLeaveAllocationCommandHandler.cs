using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands;
public class CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<CreateLeaveAllocationCommand, int>
{
    public async Task<int> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var leaveAllocation = mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
        leaveAllocation = await leaveAllocationRepository.Add(leaveAllocation);
        return leaveAllocation.Id;
    }
}
