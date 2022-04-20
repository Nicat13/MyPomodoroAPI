using System;
using System.Text.Json.Serialization;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class PomodoroSessionDetailsViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int LongBreakInterval { get; set; }
        public int PeriodCount { get; set; }
        [JsonIgnore]
        public DateTime? StatusChangeTime { get; set; }
        public double? _currentTime;
        public double? CurrentTime
        {
            get
            {
                if (StatusChangeTime != null && CurrentStatus == PomodoroStatuses.Start)
                {
                    var time = _currentTime - (TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time"))).DateTime.Subtract((DateTime)StatusChangeTime).TotalMinutes;
                    return time < 0 ? 0 : Convert.ToDouble(String.Format("{0:0.00}", time));
                }
                return Convert.ToDouble(String.Format("{0:0.00}", _currentTime));
            }
            set { _currentTime = value; }
        }
        public string SessionShareCode { get; set; }
        public PomodoroStatuses CurrentStatus { get; set; }
        public PomodoroSteps CurrentStep { get; set; }
        public UserConfigurationViewModel UserConfiguration { get; set; }
    }
}