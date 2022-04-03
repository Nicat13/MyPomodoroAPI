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

namespace MyPomodoro.Application.Features.Pomodoros.Commands.UpdatePomodoro
{
    public class UpdatePomodoroCommand : IRequest<PomodoroDetailsViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int LongBreakInterval { get; set; }
        public int PeriodCount { get; set; }
        public int Color { get; set; }
    }
    public class UpdatePomodoroCommandHandler : IRequestHandler<UpdatePomodoroCommand, PomodoroDetailsViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public UpdatePomodoroCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<PomodoroDetailsViewModel> Handle(UpdatePomodoroCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        string userId = userService.GetUserId();
                        var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(userId);
                        if (ActivePomodoroSession != null)
                        {
                            throw new HttpStatusException(new List<string> { "There is already active session." });
                        }
                        var Pomodoro = await uow.PomodoroRepository.GetByIdAsync(request.Id);
                        if (Pomodoro == null || Pomodoro.UserId != userId)
                        {
                            throw new HttpStatusException(new List<string> { "Pomodoro not found." });
                        }
                        Pomodoro = _mapper.Map<UpdatePomodoroCommand, Pomodoro>(request, Pomodoro);
                        uow.PomodoroRepository.Update(Pomodoro);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return _mapper.Map<PomodoroDetailsViewModel>(Pomodoro);
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