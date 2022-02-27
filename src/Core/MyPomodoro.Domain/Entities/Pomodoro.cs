using MyPomodoro.Domain.Common;
using System;
using System.Collections.Generic;

namespace MyPomodoro.Domain.Entities
{
    public class Pomodoro : BaseEntity
    {
        public Pomodoro()
        {
            PomodoroSessions = new HashSet<PomodoroSession>();
        }
        public int PomodoroTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int PeriodCount { get; set; }
        public int Color { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<PomodoroSession> PomodoroSessions { get; set; }
    }
}
