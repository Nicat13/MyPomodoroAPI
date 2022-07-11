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
using MyPomodoro.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace MyPomodoro.Application.Features.WebPushs.Commands.Subscribe
{
    public class SubscribeCommand : IRequest<PushSubscriptionViewModel>
    {
        public SubscriptionDTO Subscription { get; set; }
        public string DeviceId { get; set; }
    }
    public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, PushSubscriptionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        IUowContext uowContext;
        public SubscribeCommandHandler(IMapper mapper, IUowContext uowContext, IUserService userService)
        {
            _mapper = mapper;
            this.uowContext = uowContext;
            this.userService = userService;
        }
        public async Task<PushSubscriptionViewModel> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                using (var trans = uow.BeginTransaction())
                {
                    try
                    {
                        string userId = userService.GetUserId();
                        var subscription = await uow.WebPushRepository.GetPushSubscriptionByKey(userId, request.Subscription.Keys.P256Dh);
                        if (subscription != null)
                        {
                            return _mapper.Map<PushSubscriptionViewModel>(subscription);
                        }
                        subscription = new WebPushSubscription()
                        {
                            UserId = userId,
                            Endpoint = request.Subscription.Endpoint,
                            ExpirationTime = request.Subscription.ExpirationTime,
                            Auth = request.Subscription.Keys.Auth,
                            P256Dh = request.Subscription.Keys.P256Dh
                        };
                        await uow.WebPushRepository.AddAsync(subscription);
                        await uow.SaveChangesAsync();
                        uow.Commit();
                        return _mapper.Map<PushSubscriptionViewModel>(subscription);
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