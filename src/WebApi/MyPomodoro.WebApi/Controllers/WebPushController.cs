using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.WebPushs.Commands.Subscribe;
using MyPomodoro.Application.Features.WebPushs.Commands.Unsubscribe;
using MyPomodoro.Application.Features.WebPushs.Queries.GetVapidPublicKey;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebPushController : BaseController
    {
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> VapidPublicKey()
        {
            return Ok(await Mediator.Send(new GetVapidPublicKeyQuery()));
        }
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}