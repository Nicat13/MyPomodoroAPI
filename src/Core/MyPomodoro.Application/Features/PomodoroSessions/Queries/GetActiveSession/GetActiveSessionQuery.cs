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

namespace MyPomodoro.Application.Features.PomodoroSessions.Queries.GetActiveSession
{
    public class GetActiveSessionQuery : IRequest<PomodoroSessionDetailsViewModel>
    {
    }
    public class GetActiveSessionQueryHandler : IRequestHandler<GetActiveSessionQuery, PomodoroSessionDetailsViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public GetActiveSessionQueryHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }

        public async Task<PomodoroSessionDetailsViewModel> Handle(GetActiveSessionQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    var UserId = userService.GetUserId();
                    var ActivePomodoroSession = uow.PomodoroSessionRepository.GetActivePomodoroSession(UserId);
                    if (ActivePomodoroSession == null)
                    {
                        throw new HttpStatusException(new List<string> { "There is no active session." });
                    }
                    ActivePomodoroSession.UserConfiguration = _mapper.Map<UserConfigurationViewModel>(uow.UserConfigurationRepository.GetUserConfiguration(UserId));
                    return await Task.FromResult(ActivePomodoroSession);
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