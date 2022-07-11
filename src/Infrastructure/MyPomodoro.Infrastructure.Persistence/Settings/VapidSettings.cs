using System;

namespace MyPomodoro.Infrastructure.Persistence.Settings
{
    public class VapidSettings
    {
        public string Subject { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}