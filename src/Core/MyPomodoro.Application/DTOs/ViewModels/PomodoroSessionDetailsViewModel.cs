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
        public double? CurrentTime { get; set; }
        public string SessionShareCode { get; set; }
        public PomodoroStatuses CurrentStatus { get; set; }
        public PomodoroSteps CurrentStep { get; set; }
        public UserConfigurationViewModel UserConfiguration { get; set; }
    }
}