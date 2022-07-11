using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class SubscriptionDTO
    {
        public string Endpoint { get; set; }

        public double? ExpirationTime { get; set; }

        public Keys Keys { get; set; }
    }
    public class Keys
    {
        public string P256Dh { get; set; }
        public string Auth { get; set; }
    }
}