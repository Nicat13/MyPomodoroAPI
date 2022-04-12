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

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.LeaveSession
{
    public class LeaveSessionCommand : IRequest<string>
    {
        public string ConnectionId { get; set; }
    }
    public class LeaveSessionCommandHandler : IRequestHandler<LeaveSessionCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IHubContext<SessionHub> _sessionHub;
        IUowContext uowContext;
        public LeaveSessionCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService, IHubContext<SessionHub> sessionHub)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
            _sessionHub = sessionHub;
        }
        public async Task<string> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var UserId = userService.GetUserId();
                        var LatestJoinedSession = uow.PomodoroSessionRepository.GetLatestJoinedSession(UserId);
                        if (LatestJoinedSession == null || LatestJoinedSession.IsJoined != true)
                        {
                            throw new HttpStatusException(new List<string> { "There is no active session." });
                        }
                        var lastsessionparticipiant = await uow.SessionParticipiantRepository.GetByIdAsync(LatestJoinedSession.Id);
                        lastsessionparticipiant.IsJoined = false;
                        uow.SessionParticipiantRepository.Update(lastsessionparticipiant);
                        await uow.SaveChangesAsync();
                        var pomodorosession = await uow.PomodoroSessionRepository.GetByIdAsync(LatestJoinedSession.SessionId);
                        uow.Commit();
                        var userEmail = userService.GetUserEmail();
                        await _sessionHub.Clients.GroupExcept(pomodorosession.SessionShareCode, request.ConnectionId)
                                .SendAsync("ReceiveLatestLeft", new
                                {
                                    UserName = userEmail
                                });
                        await _sessionHub.Groups.RemoveFromGroupAsync(request.ConnectionId, pomodorosession.SessionShareCode);
                        return await Task.FromResult("Left successfully.");
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