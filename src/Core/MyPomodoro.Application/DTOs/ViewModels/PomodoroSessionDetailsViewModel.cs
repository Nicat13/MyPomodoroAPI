using System;
using System.Text.Json.Serialization;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class PomodoroSessionDetailsViewModel
    {
        public string Name { get; set; }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int LongBreakInterval { get; set; }
        public int PeriodCount { get; set; }
        [JsonIgnore]
        public PomodoroStatuses CurrentStatus { get; set; }
        public UserConfigurationViewModel UserConfiguration { get; set; }
    }
}