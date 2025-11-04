using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    public CreateLeaveAllocationCommandHandler(IMapper mapper,
        ILeaveAllocationRepository leaveAllocationRepository,
        ILeaveTypeRepository leaveTypeRepository,
        IUserService userService)
    {
        _mapper = mapper;
        _leaveAllocationRepository = leaveAllocationRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _userService = userService;
    }

    public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateLeaveAllocationDtoValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = "Creation failed.";
            response.Errors = validationResult.Errors.Select(s => s.ErrorMessage).ToList();
        }
        else
        {
            var leaveType = await _leaveTypeRepository.Get(request.LeaveAllocationDto.LeaveTypeId);
            var employees = await _userService.GetEmployees();
            var period = DateTime.Now.Year;
            var allocations = new List<LeaveAllocation>();
            foreach (var employee in employees)
            {
                if (await _leaveAllocationRepository.AllocationExists(employee.Id, leaveType.Id, period))
                    continue;
                allocations.Add(new LeaveAllocation()
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveType.Id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period
                });
            }

            await _leaveAllocationRepository.AddAllocations(allocations);

            response.Success = true;
            response.Message = "Allocation successful";
        }
        return response;

    }
}