using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.Pomodoros.Queries.GetUserPomodoros
{
    public class GetUserPomodorosQuery : IRequest<IEnumerable<PomodoroViewModel>>
    {
    }
    public class GetUserPomodorosQueryHandler : IRequestHandler<GetUserPomodorosQuery, IEnumerable<PomodoroViewModel>>
    {
        private readonly IUserService userService;
        IUowContext uowContext;

        public GetUserPomodorosQueryHandler(IUserService userService, IUowContext uowContext)
        {
            this.userService = userService;
            this.uowContext = uowContext;
        }

        public async Task<IEnumerable<PomodoroViewModel>> Handle(GetUserPomodorosQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    string userId = userService.GetUserId();
                    return await uow.PomodoroRepository.GetUserPomodoros(userId);
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