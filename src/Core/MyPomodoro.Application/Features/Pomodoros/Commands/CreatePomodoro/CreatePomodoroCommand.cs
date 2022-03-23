using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace MyPomodoro.Application.Features.Pomodoros.Commands.CreatePomodoro
{
    public partial class CreatePomodoroCommand : IRequest<string>
    {
        public string Name { get; set; }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int LongBreakInterval { get; set; }
        public int PeriodCount { get; set; }
        public int Color { get; set; }
    }

    public class CreatePomodoroCommandHandler : IRequestHandler<CreatePomodoroCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public CreatePomodoroCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<string> Handle(CreatePomodoroCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var Pomodoro = _mapper.Map<Pomodoro>(request);
                        Pomodoro.UserId = userService.GetUserId();
                        Pomodoro.CreateDate = DateTime.UtcNow.AddHours(4);
                        await uow.PomodoroRepository.AddAsync(Pomodoro);
                        await uow.SaveChangesAsync();
                        uow.Commit();
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
            return await Task.FromResult("Pomodoro Created.");
        }
    }
}