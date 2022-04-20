using System;

namespace MyPomodoro.Application.DTOs.ViewModels
{
    public class CreatedTaskViewModel
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatePomodoros { get; set; }
    }
}