using System;
using MyPomodoro.Application.Interfaces.Services;

namespace MyPomodoro.Infrastructure.Persistence.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset localTime => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time"));
        public DateTime NowUtc => localTime.DateTime;
    }
}