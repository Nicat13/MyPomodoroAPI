using FluentValidation;

namespace MyPomodoro.Application.Features.Pomodoros.Queries.GetPomodoroDetails
{
    public class GetPomodoroDetailsQueryValidator : AbstractValidator<GetPomodoroDetailsQuery>
    {
        public GetPomodoroDetailsQueryValidator()
        {
            RuleFor(p => p.PomodoroId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEqual(0).WithMessage("{PropertyName} is not equal 0.")
                .NotNull();
        }
    }
}