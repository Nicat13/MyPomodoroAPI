using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class UserConfigurationViewModel
    {
        public bool AutoStartPomodoros { get; set; }
        public bool AutoStartBreaks { get; set; }
        public bool EmailNotification { get; set; }
        public bool PushNotification { get; set; }
    }
}