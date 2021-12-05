using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.DTOs
{
    public class testdto : IRequest<int>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
    public class CreateTestCommandHandler : IRequestHandler<testdto, int>
    {
        IUowContext uowContext;

        public CreateTestCommandHandler(IUowContext uowContext)
        {
            this.uowContext = uowContext;
        }

        public async Task<int> Handle(testdto request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        uow.TestRepo.addDepart();
                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        uow.RollBack();
                        throw new HttpStatusException(new List<string> { ex.Message });
                    }
                    finally
                    {
                        uow.Close();
                    }
                }

            }
            return await Task.FromResult(200);
        }
    }
    public class CreateTestCommandValidator : AbstractValidator<testdto>
    {
        public CreateTestCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .NotEqual("0").WithMessage("{PropertyName} is not equal 0.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress();
        }
    }
}