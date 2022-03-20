using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.Pomodoros.Commands.CreatePomodoro
{
    public class CreatePomodoroCommandValidator : AbstractValidator<CreatePomodoroCommand>
    {
        public CreatePomodoroCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(20).WithMessage("{PropertyName} must not exceed 20 characters.");
            RuleFor(p => p.PomodoroTime)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
            RuleFor(p => p.ShortBreakTime)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
            RuleFor(p => p.LongBreakTime)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
            RuleFor(p => p.LongBreakInterval)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
            RuleFor(p => p.PeriodCount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
            RuleFor(p => p.Color)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
        }
    }
}