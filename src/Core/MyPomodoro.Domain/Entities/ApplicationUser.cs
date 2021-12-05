using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPomodoro.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Pomodoros = new HashSet<Pomodoro>();
            Tasks = new HashSet<Task>();
            UserConfigurations = new HashSet<UserConfiguration>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public virtual ICollection<Pomodoro> Pomodoros { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<UserConfiguration> UserConfigurations { get; set; }
    }
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
