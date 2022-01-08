
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs.Account;
using MyPomodoro.Application.DTOs.JWT;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> SendEmailVerification(ApplicationUser user);
        Task<string> ConfirmEmailAsync(string email, string code);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<JwtTokenDto> RevokeByRefreshToken(string token);
        Task<string> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<string> ResetPasswordAsync(ResetPasswordRequest request);
    }
}