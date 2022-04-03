using System;
using FluentValidation;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.CreateSession
{
    public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionCommandValidator()
        {
            RuleFor(p => p.PomodoroId)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                    .NotNull();
            When(p => p.SessionType == 0, () =>
            {
                RuleFor(p => p.Password)
                    .Empty();
            }).Otherwise(() =>
            {
                RuleFor(p => p.Password)
                    .MaximumLength(15).WithMessage("{PropertyName} must not exceed 15 characters.");
            });
            RuleFor(p => p.SessionType)
                    .NotNull()
                    .WithMessage("{PropertyName} is required.")
                    .IsInEnum();
        }
    }
}