using System.Security.Claims;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface IUserService
    {
        ClaimsPrincipal GetUser();
        public string GetUserId();
        public string GetUserEmail();
    }
}