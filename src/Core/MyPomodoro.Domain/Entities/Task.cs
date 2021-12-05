﻿using MyPomodoro.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyPomodoro.Domain.Entities
{
    public class Task:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatePomodoros { get; set; }
        public int TotalPomodoros { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreateDate { get; set; }
        public string PomodoroId { get; set; }
        public Pomodoro Pomodoro { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
