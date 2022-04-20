using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.Features.Tasks.Commands.CreateTask;
using MyPomodoro.Application.Features.Tasks.Commands.DeleteTask;
using MyPomodoro.Application.Features.Tasks.Queries.GetActiveSessionTasks;

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
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> ActiveSessionTasks()
        {
            return Ok(await Mediator.Send(new GetActiveSessionTasksQuery()));
        }
        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}