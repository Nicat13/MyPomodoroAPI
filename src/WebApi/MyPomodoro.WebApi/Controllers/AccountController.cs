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
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            return Ok(await _accountService.ForgotPasswordAsync(request));
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request));
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}