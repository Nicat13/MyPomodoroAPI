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

namespace MyPomodoro.Application.Features.Users.Queries.GetUserConfiguration
{
    public class GetUserConfigurationQuery : IRequest<UserConfigurationViewModel>
    {
    }
    public class GetUserConfigurationQueryHandler : IRequestHandler<GetUserConfigurationQuery, UserConfigurationViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public GetUserConfigurationQueryHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<UserConfigurationViewModel> Handle(GetUserConfigurationQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    string userId = userService.GetUserId();
                    var UserConfiguration = uow.UserConfigurationRepository.GetUserConfiguration(userId);
                    if (UserConfiguration == null)
                    {
                        throw new HttpStatusException(new List<string> { "Configuration not found." });
                    }
                    return await Task.FromResult(_mapper.Map<UserConfigurationViewModel>(UserConfiguration));
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