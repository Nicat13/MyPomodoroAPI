using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyPomodoro.Domain.Common;

namespace MyPomodoro.Domain.Entities
{
    [Table("WebPushSubscriptions")]
    public class WebPushSubscription : BaseEntity
    {
        [Required]
        public string Endpoint { get; set; }
        public double? ExpirationTime { get; set; }
        [Required]
        public string P256Dh { get; set; }
        [Required]
        public string Auth { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}