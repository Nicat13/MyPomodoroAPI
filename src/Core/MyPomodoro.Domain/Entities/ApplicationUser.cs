using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MyPomodoro.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Pomodoros = new HashSet<Pomodoro>();
            Tasks = new HashSet<Task>();
            UserConfigurations = new HashSet<UserConfiguration>();
            PomodoroSessions = new HashSet<PomodoroSession>();
            SessionParticipiants = new HashSet<SessionParticipiant>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public virtual ICollection<Pomodoro> Pomodoros { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<PomodoroSession> PomodoroSessions { get; set; }
        public virtual ICollection<UserConfiguration> UserConfigurations { get; set; }
        public virtual ICollection<SessionParticipiant> SessionParticipiants { get; set; }

    }

}
