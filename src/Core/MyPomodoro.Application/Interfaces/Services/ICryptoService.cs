using System;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface ICryptoService
    {
        public string HashPassword(string password);
    }
}