using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.Pomodoros.Commands.CreatePomodoro;

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
            command.UserId = User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            return Ok(await Mediator.Send(command));
        }
    }
}