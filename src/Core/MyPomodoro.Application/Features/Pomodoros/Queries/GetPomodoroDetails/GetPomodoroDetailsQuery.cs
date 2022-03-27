using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Application.Features.Pomodoros.Queries.GetPomodoroDetails
{
    public class GetPomodoroDetailsQuery : IRequest<PomodoroDetailsViewModel>
    {
        public int PomodoroId { get; set; }
    }
    public class GetPomodoroDetailsQueryHandler : IRequestHandler<GetPomodoroDetailsQuery, PomodoroDetailsViewModel>
    {
        private readonly IUserService userService;
        IUowContext uowContext;

        public GetPomodoroDetailsQueryHandler(IUserService userService, IUowContext uowContext)
        {
            this.userService = userService;
            this.uowContext = uowContext;
        }

        public async Task<PomodoroDetailsViewModel> Handle(GetPomodoroDetailsQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    string userId = userService.GetUserId();
                    PomodoroDetailsViewModel result = uow.PomodoroRepository.GetPomodoroDetails(userId, request.PomodoroId);
                    if (result == null)
                    {
                        throw new HttpStatusException(new List<string> { "Pomodoro not found." }, HttpStatusCode.NotFound);
                    }
                    return await Task.FromResult(result);
                }
                catch (Exception ex)
                {
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