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

namespace MyPomodoro.Application.Features.WebPushs.Commands.Unsubscribe
{
    public class UnsubscribeCommand : IRequest<PushSubscriptionViewModel>
    {
        public SubscriptionDTO Subscription { get; set; }
        public string DeviceId { get; set; }
    }
    public class UnsubscribeCommandHandler : IRequestHandler<UnsubscribeCommand, PushSubscriptionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public UnsubscribeCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<PushSubscriptionViewModel> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        string userId = userService.GetUserId();
                        var subscription = await uow.WebPushRepository.GetPushSubscriptionByKey(userId, request.Subscription.Keys.P256Dh);
                        if (subscription == null)
                        {
                            throw new HttpStatusException(new List<string> { "Subscription not found." });
                        }
                        var result = _mapper.Map<PushSubscriptionViewModel>(subscription);
                        uow.WebPushRepository.Delete(subscription);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return result;
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
            };
        }
    }
}