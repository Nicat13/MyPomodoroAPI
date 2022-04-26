using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(30).WithMessage("{PropertyName} must not exceed 30 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.EstimatePomodoros)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull()
                .LessThanOrEqualTo(999).WithMessage("{PropertyName} must not greater than 999.");
        }
    }
}