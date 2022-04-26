using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using ThreadTask = System.Threading.Tasks;

namespace MyPomodoro.Application.Features.Tasks.Commands.DoneTask
{
    public class DoneTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DoneTaskCommandHandler : IRequestHandler<DoneTaskCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public DoneTaskCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async ThreadTask.Task<bool> Handle(DoneTaskCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        string userId = userService.GetUserId();
                        var task = await uow.TaskRepository.GetTaskByIdAndUserId(request.Id, userId);
                        if (task == null)
                        {
                            throw new HttpStatusException(new List<string> { "Task not found." });
                        }
                        task.IsDone = !task.IsDone;
                        uow.TaskRepository.Update(task);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await ThreadTask.Task.FromResult(task.IsDone);
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