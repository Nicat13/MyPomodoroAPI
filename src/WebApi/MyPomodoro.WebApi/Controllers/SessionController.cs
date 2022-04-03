using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.PomodoroSessions.Commands.CreateSession;
using MyPomodoro.Application.Features.PomodoroSessions.Queries.GetActiveSession;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : BaseController
    {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> ActiveSession()
        {
            return Ok(await Mediator.Send(new GetActiveSessionQuery()));
        }
    }
}