using AutoMapper;
using HR.LeaveManagement.Application.DTOs;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries;
public class GetLeaveTypeListRequestHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<GetLeaveTypeListRequest, List<LeaveTypeDto>>
{
    public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypeListRequest request, CancellationToken cancellationToken)
    {
        var leaveTypes = await leaveTypeRepository.GetAll();
        return mapper.Map<List<LeaveTypeDto>>(leaveTypes);
    }
}
