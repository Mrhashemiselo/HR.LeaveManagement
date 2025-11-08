using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries;
public class GetLeaveAllocationListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserService _userService;
    public GetLeaveAllocationListRequestHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _userService = userService;
    }

    public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
    {
        var leaveAllocations = new List<LeaveAllocation>();
        var allocations = new List<LeaveAllocationDto>();

        if (request.IsLoggedInUser)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(f => f.Type == CustomClaimType.Uid)?.Value;
            leaveAllocations = await _unitOfWork.LeaveAllocationRepository.GetLeaveAllocationsWithDetails(userId);

            var employee = await _userService.GetEmployee(userId);
            allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
            foreach (var allocation in allocations)
            {
                allocation.Employee = employee;
            }
        }
        else
        {
            leaveAllocations = await _unitOfWork.LeaveAllocationRepository.GetLeaveAllocationsWithDetails();
            allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
            foreach (var allocation in allocations)
            {
                allocation.Employee = await _userService.GetEmployee(allocation.EmployeeId);
            }
        }
        return allocations;
    }
}
