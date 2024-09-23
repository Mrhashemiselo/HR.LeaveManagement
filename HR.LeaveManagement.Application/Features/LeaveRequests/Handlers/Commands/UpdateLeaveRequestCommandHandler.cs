using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;
public class UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<UpdateLeaveRequestCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await leaveRequestRepository.Get(request.LeaveRequestDto.Id);
        if (request.LeaveRequestDto != null)
        {
            mapper.Map(request.LeaveRequestDto, leaveRequest);
            await leaveRequestRepository.Update(leaveRequest);
        }
        else if (request.ChangeLeaveRequestApprovalDto != null)
        {
            await leaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
        }
        return Unit.Value;
    }
}