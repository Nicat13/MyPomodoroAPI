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

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.CreateSession
{
    public class CreateSessionCommand : IRequest<PomodoroSessionDetailsViewModel>
    {
        public int PomodoroId { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }
        public PomodoroSessionType SessionType { get; set; }
    }
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, PomodoroSessionDetailsViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IHubContext<SessionHub> _sessionHub;
        private readonly ICryptoService _cryptoService;

        IUowContext uowContext;
        public CreateSessionCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService, IHubContext<SessionHub> sessionHub, ICryptoService cryptoService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
            _sessionHub = sessionHub;
            _cryptoService = cryptoService;
        }
        public async Task<PomodoroSessionDetailsViewModel> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var UserId = userService.GetUserId();
                        var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(UserId);
                        var JoinedActiveSession = uow.PomodoroSessionRepository.GetJoinedActivePomodoroSession(UserId);
                        if (ActivePomodoroSession != null || JoinedActiveSession != null)
                        {
                            throw new HttpStatusException(new List<string> { "There is already active session." });
                        }
                        var Pomodoro = uow.PomodoroRepository.GetPomodoroDetails(UserId, request.PomodoroId);
                        if (Pomodoro == null)
                        {
                            throw new HttpStatusException(new List<string> { "Pomodoro not found." });
                        }
                        if (String.IsNullOrEmpty(request.Password.Trim()))
                        {
                            request.Password = null;
                        }
                        else
                        {
                            request.Password = _cryptoService.HashPassword(request.Password.Trim());
                        }
                        PomodoroSession NewSession = new PomodoroSession()
                        {
                            IsActive = true,
                            TotalPomodoroTime = 0,
                            TotalLongBreakTime = 0,
                            TotalShortBreakTime = 0,
                            CurrentPomodoroPeriod = 1,
                            CurrentStep = 0,
                            CurrentStatus = 0,
                            CurrentTime = Pomodoro.PomodoroTime,
                            Password = request.Password,
                            SessionShareCode = Guid.NewGuid().ToString(),
                            SessionType = request.SessionType,
                            SessionCreateDate = DateTime.UtcNow.AddHours(4),
                            PomodoroId = Pomodoro.Id,
                            UserId = UserId
                        };
                        if (!String.IsNullOrEmpty(request.ConnectionId))
                        {
                            await _sessionHub.Groups.AddToGroupAsync(request.ConnectionId, NewSession.SessionShareCode);
                        }
                        var result = _mapper.Map<PomodoroSessionDetailsViewModel>(Pomodoro);
                        result.CurrentStep = NewSession.CurrentStep;
                        result.CurrentStatus = NewSession.CurrentStatus;
                        result.SessionShareCode = NewSession.SessionShareCode;
                        result.CurrentTime = NewSession.CurrentTime;
                        result.UserConfiguration = _mapper.Map<UserConfigurationViewModel>(uow.UserConfigurationRepository.GetUserConfiguration(UserId));
                        await uow.PomodoroSessionRepository.AddAsync(NewSession);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await Task.FromResult(result);
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