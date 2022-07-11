using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Hubs;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.SessionAction
{
    public class SessionActionCommand : IRequest<SessionActionViewModel>
    {
        public string ConnectionId { get; set; }
    }
    public class SessionActionCommandHandler : IRequestHandler<SessionActionCommand, SessionActionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IHubContext<SessionHub> _sessionHub;
        private readonly IDateTimeService _dateTimeService;
        IUowContext uowContext;
        public SessionActionCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService, IHubContext<SessionHub> sessionHub, IDateTimeService dateTimeService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
            _sessionHub = sessionHub;
            _dateTimeService = dateTimeService;
        }
        //FIXME: WHEN CLICK ACTION BUTTON TOTALTIMES IS WRONG
        public async Task<SessionActionViewModel> Handle(SessionActionCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var UserId = userService.GetUserId();
                        var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(UserId);
                        if (ActivePomodoroSession == null)
                        {
                            throw new HttpStatusException(new List<string> { "There is no active session." });
                        }
                        var pomodorosession = await uow.PomodoroSessionRepository.GetByIdAsync(ActivePomodoroSession.Id);
                        var statuschangetime = _dateTimeService.NowUtc;
                        if (pomodorosession.CurrentStatus == PomodoroStatuses.Stop)
                        {
                            pomodorosession.CurrentStatus = PomodoroStatuses.Start;
                        }
                        else
                        {
                            pomodorosession.CurrentStatus = PomodoroStatuses.Stop;
                            if (pomodorosession.StatusChangeTime != null && pomodorosession.CurrentTime != 0)
                            {
                                var CurrentTime = pomodorosession.CurrentTime - (statuschangetime.Subtract((DateTime)pomodorosession.StatusChangeTime).TotalMinutes);
                                pomodorosession.CurrentTime = CurrentTime < 0 ? 0 : CurrentTime;
                            }
                        }

                        pomodorosession.StatusChangeTime = statuschangetime;
                        uow.PomodoroSessionRepository.Update(pomodorosession);
                        await uow.SaveChangesAsync();
                        SessionActionViewModel result = new SessionActionViewModel()
                        {
                            SessionStatus = pomodorosession.CurrentStatus
                        };
                        uow.Commit();
                        await _sessionHub.Clients.GroupExcept(pomodorosession.SessionShareCode, request.ConnectionId)
                                .SendAsync("ReceiveSessionAction", result);
                        return result;
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
        }
    }
}