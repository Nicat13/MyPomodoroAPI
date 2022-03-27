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

namespace MyPomodoro.Application.Features.Users.Commands.UpdateUserConfiguration
{
    public class UpdateUserConfigurationCommand : IRequest<string>
    {
        public bool AutoStartPomodoros { get; set; }
        public bool AutoStartBreaks { get; set; }
        public bool EmailNotification { get; set; }
        public bool PushNotification { get; set; }
    }
    public class UpdateUserConfigurationCommandHandler : IRequestHandler<UpdateUserConfigurationCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public UpdateUserConfigurationCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<string> Handle(UpdateUserConfigurationCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        string userId = userService.GetUserId();
                        var UserConfiguration = uow.UserConfigurationRepository.GetUserConfiguration(userId);
                        if (UserConfiguration == null)
                        {
                            throw new HttpStatusException(new List<string> { "Configuration not found." });
                        }
                        UserConfiguration = _mapper.Map<UpdateUserConfigurationCommand, UserConfiguration>(request, UserConfiguration);
                        uow.UserConfigurationRepository.Update(UserConfiguration);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return await Task.FromResult("Configuration Updated.");
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