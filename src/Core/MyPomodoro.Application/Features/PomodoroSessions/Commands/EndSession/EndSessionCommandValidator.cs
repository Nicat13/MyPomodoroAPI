using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.EndSession
{
    public class EndSessionCommandValidator : AbstractValidator<EndSessionCommand>
    {
        public EndSessionCommandValidator()
        {
            RuleFor(p => p.ConnectionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}