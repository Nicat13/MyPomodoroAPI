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
using MyPomodoro.Domain.Entities;
using MyPomodoro.Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.JoinSession
{
    public class JoinSessionCommand : IRequest<JoinedPomodoroSessionDetailsViewModel>
    {
        public string ConnectionId { get; set; }
        public string Password { get; set; }
        public string sessionCode { get; set; }
    }
    public class JoinSessionCommandHandler : IRequestHandler<JoinSessionCommand, JoinedPomodoroSessionDetailsViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly ICryptoService cryptoService;
        private readonly IHubContext<SessionHub> _sessionHub;
        private readonly IDateTimeService _dateTimeService;
        IUowContext uowContext;
        public JoinSessionCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService, IHubContext<SessionHub> sessionHub, ICryptoService cryptoService, IDateTimeService dateTimeService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
            _sessionHub = sessionHub;
            this.cryptoService = cryptoService;
            _dateTimeService = dateTimeService;
        }
        public async Task<JoinedPomodoroSessionDetailsViewModel> Handle(JoinSessionCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var UserId = userService.GetUserId();
                        var ActiveSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(UserId);
                        var LatestJoinedSession = uow.PomodoroSessionRepository.GetLatestJoinedSession(UserId);
                        if (ActiveSession != null || (LatestJoinedSession != null && LatestJoinedSession.IsJoined == true))
                        {
                            throw new HttpStatusException(new List<string> { "There is already active session." });
                        }
                        var sessionModel = await uow.PomodoroSessionRepository.GetSessionBySessionShareCode(request.sessionCode);
                        if (sessionModel == null || sessionModel.SessionType != PomodoroSessionType.Public)
                        {
                            throw new HttpStatusException(new List<string> { "Session not found." });
                        }
                        if ((!String.IsNullOrEmpty(sessionModel.Password) && String.IsNullOrEmpty(request.Password.Trim())) ||
                        (!String.IsNullOrEmpty(sessionModel.Password) && sessionModel.Password != cryptoService.HashPassword(request.Password.Trim())))
                        {
                            throw new HttpStatusException(new List<string> { "Invalid Password." });
                        }
                        if (sessionModel.CurrentStatus != PomodoroStatuses.Stop)
                        {
                            throw new HttpStatusException(new List<string> { "Session already started." });
                        }
                        if (LatestJoinedSession != null && sessionModel.Id == LatestJoinedSession.SessionId)
                        {
                            var lastsessionparticipiant = await uow.SessionParticipiantRepository.GetByIdAsync(LatestJoinedSession.Id);
                            lastsessionparticipiant.IsJoined = true;
                            uow.SessionParticipiantRepository.Update(lastsessionparticipiant);
                        }
                        else
                        {
                            SessionParticipiant participiant = new SessionParticipiant()
                            {
                                JoinDate = _dateTimeService.NowUtc,
                                UserId = UserId,
                                SessionId = sessionModel.Id,
                                IsJoined = true
                            };
                            await uow.SessionParticipiantRepository.AddAsync(participiant);
                        }
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        var userName = userService.GetUserName();
                        await _sessionHub.Groups.AddToGroupAsync(request.ConnectionId, sessionModel.SessionShareCode);
                        await _sessionHub.Clients.GroupExcept(sessionModel.SessionShareCode, request.ConnectionId)
                                .SendAsync("ReceiveLatestJoined", new
                                {
                                    UserName = userName
                                });
                        var result = await Task.FromResult(uow.PomodoroSessionRepository.GetJoinedActivePomodoroSessionDetails(UserId));
                        var sessionAuthorSettings = uow.UserConfigurationRepository.GetUserConfiguration(sessionModel.UserId);
                        result.AutoStartPomodoro = sessionAuthorSettings.AutoStartPomodoros;
                        result.AutoStartBreaks = sessionAuthorSettings.AutoStartBreaks;
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