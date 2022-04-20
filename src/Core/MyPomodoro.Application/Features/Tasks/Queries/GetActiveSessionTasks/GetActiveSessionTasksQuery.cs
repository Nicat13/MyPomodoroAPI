using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.Tasks.Queries.GetActiveSessionTasks
{
    public class GetActiveSessionTasksQuery : IRequest<IEnumerable<CreatedTaskViewModel>>
    {
    }
    public class GetActiveSessionTasksQueryHandler : IRequestHandler<GetActiveSessionTasksQuery, IEnumerable<CreatedTaskViewModel>>
    {
        private readonly IUserService userService;
        IUowContext uowContext;

        public GetActiveSessionTasksQueryHandler(IUserService userService, IUowContext uowContext)
        {
            this.userService = userService;
            this.uowContext = uowContext;
        }
        public async Task<IEnumerable<CreatedTaskViewModel>> Handle(GetActiveSessionTasksQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    var userId = userService.GetUserId();
                    var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(userId);
                    var JoinedActiveSession = uow.PomodoroSessionRepository.GetJoinedActivePomodoroSession(userId);
                    if (ActivePomodoroSession == null && JoinedActiveSession == null)
                    {
                        throw new HttpStatusException(new List<string> { "There is not active session." });
                    }
                    int sessionId = ActivePomodoroSession != null ? ActivePomodoroSession.Id : JoinedActiveSession.Id;
                    return await uow.TaskRepository.GetSessionTasks(sessionId, userId);
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