using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class SessionLobbyViewModel
    {
        public string UserName { get; set; }
        public string PomodoroName { get; set; }
        public bool HasPass { get; set; }
        public string SessionShareCode { get; set; }
    }
}