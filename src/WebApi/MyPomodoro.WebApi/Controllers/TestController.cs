using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs;

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
        [HttpGet]
        public IActionResult get([FromQuery] testgetmodel model)
        {
            return Ok(model);
        }
        [HttpGet("[action]")]
        public IActionResult GetIP()
        {
            return Ok(GenerateIPAddress());
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }

    public class testgetmodel
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
