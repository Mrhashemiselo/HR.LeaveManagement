using AutoMapper;
using FluentValidation.Results;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateLeaveRequestCommandHandler(IMapper mapper,
        ILeaveRequestRepository leaveRequestRepository,
        ILeaveTypeRepository leaveTypeRepository,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor,
        ILeaveAllocationRepository leaveAllocationRepository)
    {
        _mapper = mapper;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _emailSender = emailSender;
        _httpContextAccessor = httpContextAccessor;
        _leaveAllocationRepository = leaveAllocationRepository;
    }

    public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateLeaveRequestDtoValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);
        var userId = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(f => f.Type == "uid")?.Value;

        var allocation = await _leaveAllocationRepository.GetUserAllocations(userId, request.LeaveRequestDto.LeaveTypeId);
        var daysRequested = (int)(request.LeaveRequestDto.EndDate - request.LeaveRequestDto.StartDate).TotalDays;

        if (daysRequested > allocation.NumberOfDays)
        {
            validationResult.Errors
                .Add(new ValidationFailure(nameof(request.LeaveRequestDto.EndDate),
                "You do not have enough days for this request."));
        }

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = "Request failed.";
            response.Errors = validationResult.Errors.Select(a => a.ErrorMessage).ToList();
        }
        else
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(request.LeaveRequestDto);
            leaveRequest.RequestingEmployeeId = userId;
            leaveRequest = await _leaveRequestRepository.Add(leaveRequest);
            response.Success = true;
            response.Message = "Creation successful.";
            response.Id = leaveRequest.Id;

            var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            var email = new Email()
            {
                To = emailAddress,
                Body =
                    $"Your leave request for {request.LeaveRequestDto.StartDate} to {request.LeaveRequestDto.EndDate}" +
                    $"has been submitted successfully.",
                Subject = "Leave request submitted."
            };
            try
            {
                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                //
            }
        }
        return response;
    }
}