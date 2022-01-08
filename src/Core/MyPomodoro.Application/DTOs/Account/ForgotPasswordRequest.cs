using System.ComponentModel.DataAnnotations;

namespace MyPomodoro.Application.DTOs.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}