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

namespace MyPomodoro.Application.Features.PomodoroSessions.Queries.GetJoinedSession
{
    public class GetJoinedSessionQuery : IRequest<JoinedPomodoroSessionViewModel>
    {
    }
    public class GetJoinedSessionQueryHandler : IRequestHandler<GetJoinedSessionQuery, JoinedPomodoroSessionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public GetJoinedSessionQueryHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<JoinedPomodoroSessionViewModel> Handle(GetJoinedSessionQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    var UserId = userService.GetUserId();
                    var JoinedPomodoroSession = uow.PomodoroSessionRepository.GetJoinedActivePomodoroSession(UserId);
                    if (JoinedPomodoroSession == null)
                    {
                        throw new HttpStatusException(new List<string> { "There is no joined session." });
                    }
                    return await Task.FromResult(JoinedPomodoroSession);
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