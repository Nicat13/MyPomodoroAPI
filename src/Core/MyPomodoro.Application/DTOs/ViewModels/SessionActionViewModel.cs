using System;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class SessionActionViewModel
    {
        public PomodoroStatuses SessionStatus { get; set; }
    }
}