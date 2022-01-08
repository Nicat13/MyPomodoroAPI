using System;

namespace MyPomodoro.Application.DTOs.JWT
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }

        public RefreshTokenDto(string token, DateTimeOffset expires)
        {
            Token = token;
            Expires = expires;
        }
    }
}