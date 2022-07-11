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

namespace MyPomodoro.Application.Features.PomodoroSessions.Queries.GetSessionLobbies
{
    public class GetSessionLobbiesQuery : IRequest<List<SessionLobbyViewModel>>
    {
    }
    public class GetSessionLobbiesQueryHandler : IRequestHandler<GetSessionLobbiesQuery, List<SessionLobbyViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public GetSessionLobbiesQueryHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<List<SessionLobbyViewModel>> Handle(GetSessionLobbiesQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    var UserId = userService.GetUserId();
                    var sessionLobbies = uow.PomodoroSessionRepository.GetSessionLobbies(UserId);
                    return await Task.FromResult(sessionLobbies);
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