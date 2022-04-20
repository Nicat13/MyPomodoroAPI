using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(p => p.Id)
                        .NotEmpty().WithMessage("{PropertyName} is required.")
                        .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                        .NotNull();
        }
    }
}