using MyPomodoro.Domain.Common;
using MyPomodoro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyPomodoro.Domain.Entities
{
    public class Pomodoro : BaseEntity
    {
        public Pomodoro()
        {
            Tasks = new HashSet<Task>();
        }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int PeriodCount { get; set; }
        public PomodoroSteps CurrentStep { get; set; }
        public PomodoroStatuses CurrentStatus { get; set; }
        public DateTime? CurrentTime { get; set; }
        public DateTime? StatusChangeTime { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }

    }
}
