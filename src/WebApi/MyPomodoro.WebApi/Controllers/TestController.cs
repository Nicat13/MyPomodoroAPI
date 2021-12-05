using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs;
using System;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> post(testdto command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
