using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class JoinedPomodoroSessionViewModel
    {
        public string UserName { get; set; }
        public string PomodoroName { get; set; }
        public string SessionShareCode { get; set; }
    }
}