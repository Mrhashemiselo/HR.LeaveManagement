using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;
public class CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<CreateLeaveRequestCommand, int>
{
    public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = mapper.Map<LeaveRequest>(request.LeaveRequestDto);
        leaveRequest = await leaveRequestRepository.Add(leaveRequest);
        return leaveRequest.Id;
    }
}
