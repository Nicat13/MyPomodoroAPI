using System;
using FluentValidation;

namespace MyPomodoro.Application.Features.WebPushs.Commands.Subscribe
{
    public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
    {
        public SubscribeCommandValidator()
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