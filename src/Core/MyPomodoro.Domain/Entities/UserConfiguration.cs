﻿using MyPomodoro.Domain.Common;

namespace MyPomodoro.Domain.Entities
{
    public class UserConfiguration : BaseEntity
    {
        public bool AutoStartPomodoros { get; set; }
        public bool AutoStartBreaks { get; set; }
        public int LongBreakInterval { get; set; }
        public int PomodoroPeriodCount { get; set; }
        public int Pomodoro { get; set; }
        public int ShortBreak { get; set; }
        public int LongBreak { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
