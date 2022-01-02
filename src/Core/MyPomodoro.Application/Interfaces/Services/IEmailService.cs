using System;
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs.Email;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}