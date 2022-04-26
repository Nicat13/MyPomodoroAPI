using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using MediatR;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Domain.Entities;
using ThreadTask = System.Threading.Tasks;

namespace MyPomodoro.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest<CreatedTaskViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatePomodoros { get; set; }
    }
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, CreatedTaskViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public UpdateTaskCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }

        public async ThreadTask.Task<CreatedTaskViewModel> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
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
                        request.Description = String.IsNullOrEmpty(request.Description.Trim()) == true ? null : request.Description;
                        task = _mapper.Map<UpdateTaskCommand, Task>(request, task);
                        uow.TaskRepository.Update(task);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await ThreadTask.Task.FromResult(_mapper.Map<CreatedTaskViewModel>(task));
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