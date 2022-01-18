using System;
using System.ComponentModel.DataAnnotations.Schema;
using MyPomodoro.Domain.Common;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Domain.Entities
{
    [Table("PomodoroSessions")]
    public class PomodoroSession : BaseEntity
    {
        public double TotalPomodoroTime { get; set; }
        public double TotalShortBreakTime { get; set; }
        public double TotalLongBreakTime { get; set; }
        public int CurrentPomodoroPeriod { get; set; }
        public PomodoroSteps CurrentStep { get; set; }
        public PomodoroStatuses CurrentStatus { get; set; }
        public int? CurrentTime { get; set; }
        public DateTime? StatusChangeTime { get; set; }
        public DateTime SessionCreateDate { get; set; }
        public bool IsActive { get; set; }
        public int PomodoroId { get; set; }
        public Pomodoro Pomodoro { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}