using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.Users.Commands.UpdateUserConfiguration;
using MyPomodoro.Application.Features.Users.Queries.GetUserConfiguration;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateUserConfiguration([FromBody] UpdateUserConfigurationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> UserConfiguration()
        {
            return Ok(await Mediator.Send(new GetUserConfigurationQuery()));
        }
    }
}