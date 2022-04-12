using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class LatestJoinedSessionViewModel
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public bool IsJoined { get; set; }
    }
}