using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries;
public class GetLeaveRequestListRequestHandler(ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestDto>>
{
    public async Task<List<LeaveRequestDto>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        var leaveRequests = await leaveRequestRepository.GetAll();
        return mapper.Map<List<LeaveRequestDto>>(leaveRequests);
    }
}
