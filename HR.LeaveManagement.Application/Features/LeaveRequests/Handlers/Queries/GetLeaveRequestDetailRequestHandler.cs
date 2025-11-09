using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries;

public class GetLeaveRequestDetailRequestHandler : IRequestHandler<GetLeaveRequestDetailRequest, LeaveRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public GetLeaveRequestDetailRequestHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<LeaveRequestDto> Handle(GetLeaveRequestDetailRequest request, CancellationToken cancellationToken)
    {
        var source = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestWithDetails(request.Id);
        var leaveRequest = _mapper.Map<LeaveRequestDto>(source);
        leaveRequest.Employee = await _userService.GetEmployee(leaveRequest.RequestingEmployeeId);
        return leaveRequest;
        //var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestWithDetails(request.Id);
        //return _mapper.Map<LeaveRequestDto>(leaveRequest);
    }
}