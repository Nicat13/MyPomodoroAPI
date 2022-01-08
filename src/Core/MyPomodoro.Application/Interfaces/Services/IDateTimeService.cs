using System;
using System.Threading.Tasks;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface IDateTimeService
    {
        Task<DateTime> GetLocalTime(string ipAddress);
    }
}