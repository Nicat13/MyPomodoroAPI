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
using MyPomodoro.Domain.Entities;
using MyPomodoro.Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace MyPomodoro.Application.Features.PomodoroSessions.Commands.CreateSession
{
    public class CreateSessionCommand : IRequest<PomodoroSessionDetailsViewModel>
    {
        public int PomodoroId { get; set; }
        public string Password { get; set; }
        public PomodoroSessionType SessionType { get; set; }
    }
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, PomodoroSessionDetailsViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public CreateSessionCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
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
                        if (ActivePomodoroSession != null)
                        {
                            throw new HttpStatusException(new List<string> { "There is already active session." });
                        }
                        var Pomodoro = uow.PomodoroRepository.GetPomodoroDetails(UserId, request.PomodoroId);
                        if (Pomodoro == null)
                        {
                            throw new HttpStatusException(new List<string> { "Pomodoro not found." });
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
                            Password = String.IsNullOrEmpty(request.Password.Trim()) == true ? null : request.Password.Trim(),
                            SessionShareCode = Guid.NewGuid().ToString(),
                            SessionType = request.SessionType,
                            SessionCreateDate = DateTime.UtcNow.AddHours(4),
                            PomodoroId = Pomodoro.Id,
                            UserId = UserId
                        };
                        var result = _mapper.Map<PomodoroSessionDetailsViewModel>(Pomodoro);
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