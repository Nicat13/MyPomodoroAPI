using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, string>
    {
        private readonly IUserService userService;
        IUowContext uowContext;
        public DeleteTaskCommandHandler(IUowContext uowContext, IUserService userService)
        {
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<string> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
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

                        uow.TaskRepository.Delete(task);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await Task.FromResult("Task Deleted.");
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