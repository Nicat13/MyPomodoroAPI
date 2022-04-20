using System;
using System.Text.Json.Serialization;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class JoinedPomodoroSessionViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PomodoroName { get; set; }
        public string SessionShareCode { get; set; }
    }
}