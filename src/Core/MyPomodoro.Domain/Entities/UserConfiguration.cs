using MyPomodoro.Domain.Common;

namespace MyPomodoro.Domain.Entities
{
    public class UserConfiguration : BaseEntity
    {
        public bool AutoStartPomodoros { get; set; }
        public bool AutoStartBreaks { get; set; }
        public bool EmailNotification { get; set; }
        public bool PushNotification { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
