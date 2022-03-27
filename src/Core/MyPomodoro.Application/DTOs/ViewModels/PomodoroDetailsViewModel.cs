using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class PomodoroDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int LongBreakInterval { get; set; }
        public int PeriodCount { get; set; }
        public int Color { get; set; }
    }
}