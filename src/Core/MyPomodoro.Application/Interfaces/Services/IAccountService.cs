
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs.Account;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> SendEmailVerification(ApplicationUser user);
        Task<string> ConfirmEmailAsync(string email, string code);
    }
}