using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Domain.Entities;
using ThreadTask = System.Threading.Tasks;

namespace MyPomodoro.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatePomodoros { get; set; }
    }
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IDateTimeService _dateTimeService;
        IUowContext uowContext;
        public CreateTaskCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService, IDateTimeService dateTimeService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
            _dateTimeService = dateTimeService;
        }
        public async ThreadTask.Task<string> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var userId = userService.GetUserId();
                        var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(userId);
                        var JoinedActiveSession = uow.PomodoroSessionRepository.GetJoinedActivePomodoroSession(userId);
                        if (ActivePomodoroSession == null && JoinedActiveSession == null)
                        {
                            throw new HttpStatusException(new List<string> { "There is not active session." });
                        }
                        request.Description = String.IsNullOrEmpty(request.Description.Trim()) == true ? null : request.Description;
                        var task = _mapper.Map<Task>(request);
                        task.UserId = userId;
                        task.PomodoroSessionId = ActivePomodoroSession != null ? ActivePomodoroSession.Id : JoinedActiveSession.Id;
                        task.CreateDate = _dateTimeService.NowUtc;
                        await uow.TaskRepository.AddAsync(task);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await ThreadTask.Task.FromResult("Task Created.");
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