using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.PomodoroSessions.Queries.GetSessionParticipiants
{
    public class GetSessionParticipiantsQuery : IRequest<List<SessionParticipiantViewModel>>
    {
    }
    public class GetSessionParticipiantsQueryHandler : IRequestHandler<GetSessionParticipiantsQuery, List<SessionParticipiantViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public GetSessionParticipiantsQueryHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<List<SessionParticipiantViewModel>> Handle(GetSessionParticipiantsQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    var userId = userService.GetUserId();
                    var sessionId = 0;
                    var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(userId);
                    var JoinedActiveSession = uow.PomodoroSessionRepository.GetJoinedActivePomodoroSession(userId);
                    sessionId = ActivePomodoroSession != null ? ActivePomodoroSession.Id : JoinedActiveSession != null ? JoinedActiveSession.Id : 0;
                    if (sessionId == 0)
                    {
                        throw new HttpStatusException(new List<string> { "There is not active session." });
                    }
                    return await Task.FromResult(uow.PomodoroSessionRepository.GetSessionParticipiants(sessionId));
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