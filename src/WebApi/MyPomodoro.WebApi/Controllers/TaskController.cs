using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.Tasks.Commands.CreateTask;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}