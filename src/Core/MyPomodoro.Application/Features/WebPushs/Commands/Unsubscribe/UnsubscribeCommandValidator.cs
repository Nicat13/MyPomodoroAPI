using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.WebPushs.Commands.Unsubscribe
{
    public class UnsubscribeCommandValidator : AbstractValidator<UnsubscribeCommand>
    {
        public UnsubscribeCommandValidator()
        {
            RuleFor(p => p.Subscription.Keys.Auth)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Subscription.Keys.P256Dh)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Subscription.Endpoint)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}