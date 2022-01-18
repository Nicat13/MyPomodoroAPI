using System;

namespace MyPomodoro.Application.Interfaces.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
        DateTimeOffset localTime { get; }
    }
}