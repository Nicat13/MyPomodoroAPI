using System;
using System.ComponentModel.DataAnnotations.Schema;
using MyPomodoro.Domain.Common;

namespace MyPomodoro.Domain.Entities
{
    [Table("SessionParticipiants")]
    public class SessionParticipiant : BaseEntity
    {
        public DateTime JoinDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int SessionId { get; set; }
        public PomodoroSession Session { get; set; }
    }
}