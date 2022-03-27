using FluentValidation;

namespace MyPomodoro.Application.Features.Pomodoros.Commands.DeletePomodoro
{
    public class DeletePomodoroCommandValidator : AbstractValidator<DeletePomodoroCommand>
    {
        public DeletePomodoroCommandValidator()
        {
            RuleFor(p => p.PomodoroId)
                        .NotEmpty().WithMessage("{PropertyName} is required.")
                        .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                        .NotNull();
        }
    }
}