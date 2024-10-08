﻿using FluentValidation;

namespace HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
public class ILeaveTypeDtoValidator : AbstractValidator<ILeaveTypeDto>
{
    public ILeaveTypeDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.S");

        RuleFor(a => a.DefaultDays)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be at least 1.")
            .LessThan(100).WithMessage("{PropertyName} must be less than {ComparisonValue}");
    }
}
