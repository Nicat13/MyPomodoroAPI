using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Hubs;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.EndSession
{
    public class EndSessionCommand : IRequest<string>
    {
        public string ConnectionId { get; set; }
    }
    public class EndSessionCommandHandler : IRequestHandler<EndSessionCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IHubContext<SessionHub> _sessionHub;
        IUowContext uowContext;
        public EndSessionCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService, IHubContext<SessionHub> sessionHub)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
            _sessionHub = sessionHub;
        }
        public async Task<string> Handle(EndSessionCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var UserId = userService.GetUserId();
                        var ActiveSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(UserId);
                        if (ActiveSession == null)
                        {
                            throw new HttpStatusException(new List<string> { "There is no active session." });
                        }
                        var sessionModel = await uow.PomodoroSessionRepository.GetByIdAsync(ActiveSession.Id);
                        sessionModel.IsActive = false;
                        uow.PomodoroSessionRepository.Update(sessionModel);
                        await uow.SaveChangesAsync();
                        if (sessionModel.SessionType == PomodoroSessionType.Public)
                        {
                            await _sessionHub.Clients.GroupExcept(sessionModel.SessionShareCode, request.ConnectionId)
                                                    .SendAsync("SessionEnded", new
                                                    {
                                                        StatusCode = true,
                                                    });
                            await _sessionHub.Groups.RemoveFromGroupAsync(request.ConnectionId, sessionModel.SessionShareCode);
                        }
                        uow.Commit();
                        return await Task.FromResult("Session Ended");
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