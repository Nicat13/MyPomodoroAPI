using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.WebPushs.Queries.GetVapidPublicKey
{
    public class GetVapidPublicKeyQuery : IRequest<string>
    {
    }
    public class GetVapidPublicKeyQueryHandler : IRequestHandler<GetVapidPublicKeyQuery, string>
    {
        private readonly IUserService userService;
        IUowContext uowContext;

        public GetVapidPublicKeyQueryHandler(IUserService userService, IUowContext uowContext)
        {
            this.userService = userService;
            this.uowContext = uowContext;
        }
        public async Task<string> Handle(GetVapidPublicKeyQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    return await Task.FromResult(uow.WebPushRepository.GetVapidPublicKey());
                }
                catch (Exception ex)
                {
                    throw new HttpStatusException(new List<string> { ex.Message });
                }
                finally
                {
                    uow.Close();
                }
            }
        }
    }
}