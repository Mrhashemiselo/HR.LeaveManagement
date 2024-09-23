using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries;
public class GetLeaveTypeDetailRequestHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<GetLeaveTypeDetailRequest, LeaveTypeDto>
{
    public async Task<LeaveTypeDto> Handle(GetLeaveTypeDetailRequest request, CancellationToken cancellationToken)
    {
        var leaveType = await leaveTypeRepository.Get(request.Id);
        return mapper.Map<LeaveTypeDto>(leaveType);
    }
}
