using System;
using System.Text;
using MyPomodoro.Application.Interfaces.Services;

namespace MyPomodoro.Infrastructure.Persistence.Services
{
    public class CryptoService : ICryptoService
    {
        public string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            string hashedpass = Convert.ToBase64String(hashBytes);
            return hashedpass;
        }
    }
}