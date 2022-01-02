using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyPomodoro.Application.DTOs.Account;
using MyPomodoro.Application.DTOs.Email;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.WebApi.StartupInjections.Validations;

namespace MyPomodoro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            return Ok(await _accountService.RegisterAsync(request));
        }
        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
        {
            return Ok(await _accountService.ConfirmEmailAsync(request.Email, request.Code));
        }
    }
}