﻿using FluentValidation;

namespace HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
public class UpdateLeaveTypeDtoValidator : AbstractValidator<LeaveTypeDto>
{
    public UpdateLeaveTypeDtoValidator()
    {
        Include(new ILeaveTypeDtoValidator());

        RuleFor(s => s.Id)
            .NotNull().WithMessage("{PropertyName} must be present");
    }
}
