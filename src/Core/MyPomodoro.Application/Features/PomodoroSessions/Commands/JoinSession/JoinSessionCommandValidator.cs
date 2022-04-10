using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.JoinSession
{
    public class JoinSessionCommandValidator : AbstractValidator<JoinSessionCommand>
    {
        public JoinSessionCommandValidator()
        {
            RuleFor(p => p.ConnectionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.sessionCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(40).WithMessage("{PropertyName} must not exceed 40 characters.");
            RuleFor(p => p.Password)
                .MaximumLength(15).WithMessage("{PropertyName} must not exceed 15 characters.");
        }
    }
}