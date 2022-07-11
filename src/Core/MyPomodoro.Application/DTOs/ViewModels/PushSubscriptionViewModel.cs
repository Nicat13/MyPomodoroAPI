using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class PushSubscriptionViewModel
    {
        public string Endpoint { get; set; }
        public double? ExpirationTime { get; set; }
        public string P256Dh { get; set; }
        public string Auth { get; set; }
    }
}