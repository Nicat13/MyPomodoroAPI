using System;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class JoinedPomodoroSessionDetailsViewModel
    {
        public string UserName { get; set; }
        public string PomodoroName { get; set; }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int LongBreakInterval { get; set; }
        public int PeriodCount { get; set; }
        public double? _currentTime;
        public double? CurrentTime
        {
            get
            {
                return Convert.ToDouble(String.Format("{0:0.00}", _currentTime));
            }
            set { _currentTime = value; }
        }
        public string SessionShareCode { get; set; }
        public PomodoroStatuses CurrentStatus { get; set; }
        public PomodoroSteps CurrentStep { get; set; }
    }
}