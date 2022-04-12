using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.LeaveSession
{
    public class LeaveSessionCommandValidator : AbstractValidator<LeaveSessionCommand>
    {
        public LeaveSessionCommandValidator()
        {
            RuleFor(p => p.ConnectionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}