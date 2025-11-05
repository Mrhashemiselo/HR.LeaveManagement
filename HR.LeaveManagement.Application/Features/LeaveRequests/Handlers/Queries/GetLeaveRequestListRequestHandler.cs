using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries;

public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDto>>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserService _userService;

    public GetLeaveRequestListRequestHandler(ILeaveRequestRepository leaveRequestRepository,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        IUserService userService)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _userService = userService;
    }
    public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        var leaveRequests = new List<LeaveRequest>();
        var requests = new List<LeaveRequestListDto>();

        if (request.IsLoggedInUser)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(f => f.Type == CustomClaimType.Uid)?.Value;
            leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails(userId);
            var employee = await _userService.GetEmployee(userId);
            requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
            foreach (var req in requests)
            {
                req.Employee = employee;
            }
        }
        else
        {
            leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
            requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
            foreach (var req in requests)
            {
                req.Employee = await _userService.GetEmployee(req.RequestingEmployeeId);
            }
        }
        return requests;
    }
}