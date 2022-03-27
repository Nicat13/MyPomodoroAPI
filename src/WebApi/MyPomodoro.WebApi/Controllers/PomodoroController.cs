using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.Pomodoros.Commands.CreatePomodoro;
using MyPomodoro.Application.Features.Pomodoros.Commands.DeletePomodoro;
using MyPomodoro.Application.Features.Pomodoros.Commands.UpdatePomodoro;
using MyPomodoro.Application.Features.Pomodoros.Queries.GetPomodoroColors;
using MyPomodoro.Application.Features.Pomodoros.Queries.GetPomodoroDetails;
using MyPomodoro.Application.Features.Pomodoros.Queries.GetUserPomodoros;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PomodoroController : BaseController
    {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreatePomodoro([FromBody] CreatePomodoroCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> UserPomodoros()
        {
            return Ok(await Mediator.Send(new GetUserPomodorosQuery()));
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> PomodoroColors()
        {
            return Ok(await Mediator.Send(new GetPomodoroColorsQuery()));
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> PomodoroDetails([FromQuery] GetPomodoroDetailsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdatePomodoro([FromBody] UpdatePomodoroCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeletePomodoro([FromBody] DeletePomodoroCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}