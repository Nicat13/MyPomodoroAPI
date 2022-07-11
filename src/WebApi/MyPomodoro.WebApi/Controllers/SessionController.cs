using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.PomodoroSessions.Commands.CreateSession;
using MyPomodoro.Application.Features.PomodoroSessions.Commands.EndSession;
using MyPomodoro.Application.Features.PomodoroSessions.Commands.JoinSession;
using MyPomodoro.Application.Features.PomodoroSessions.Commands.LeaveSession;
using MyPomodoro.Application.Features.PomodoroSessions.Commands.SessionAction;
using MyPomodoro.Application.Features.PomodoroSessions.Queries.GetActiveSession;
using MyPomodoro.Application.Features.PomodoroSessions.Queries.GetJoinedSession;
using MyPomodoro.Application.Features.PomodoroSessions.Queries.GetJoinedSessionDetails;
using MyPomodoro.Application.Features.PomodoroSessions.Queries.GetSessionLobbies;
using MyPomodoro.Application.Features.PomodoroSessions.Queries.GetSessionParticipiants;

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
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> JoinedSession()
        {
            return Ok(await Mediator.Send(new GetJoinedSessionQuery()));
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> JoinedSessionDetails()
        {
            return Ok(await Mediator.Send(new GetJoinedSessionDetailsQuery()));
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> SessionLobbies()
        {
            return Ok(await Mediator.Send(new GetSessionLobbiesQuery()));
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> SessionParticipiants()
        {
            return Ok(await Mediator.Send(new GetSessionParticipiantsQuery()));
        }
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> EndSession([FromBody] EndSessionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> JoinSession([FromBody] JoinSessionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> LeaveSession([FromBody] LeaveSessionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> SessionAction([FromBody] SessionActionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}