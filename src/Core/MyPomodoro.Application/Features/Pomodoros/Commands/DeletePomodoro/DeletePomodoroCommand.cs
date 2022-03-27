using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.Pomodoros.Commands.DeletePomodoro
{
    public class DeletePomodoroCommand : IRequest<string>
    {
        public int PomodoroId { get; set; }
    }
    public class DeletePomodoroCommandHandler : IRequestHandler<DeletePomodoroCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public DeletePomodoroCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<string> Handle(DeletePomodoroCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        var Pomodoro = await uow.PomodoroRepository.GetByIdAsync(request.PomodoroId);
                        string userId = userService.GetUserId();
                        if (Pomodoro == null || Pomodoro.UserId != userId)
                        {
                            throw new HttpStatusException(new List<string> { "Pomodoro not found." });
                        }
                        Pomodoro.IsDeleted = true;
                        uow.PomodoroRepository.Update(Pomodoro);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await Task.FromResult("Pomodoro Deleted.");
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