using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.Tasks.Commands.DoneTask
{
    public class DoneTaskCommandValidator : AbstractValidator<DoneTaskCommand>
    {
        public DoneTaskCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
        }
    }
}