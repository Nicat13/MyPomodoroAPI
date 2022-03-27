
using System.Text.Json.Serialization;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class PomodoroViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PomodoroTime { get; set; }
        [JsonIgnore]
        public int Color { get; set; }
        public string BgColor { get; set; }
        public string TxtColor { get; set; }
    }
}