
namespace MyPomodoro.Infrastructure.Persistence.Settings
{
    public class APIAppSettings
    {
        public string ConnectionString { get; set; }
        public string ClientAppOrigin { get; set; }
        public MailSettings MailSettings { get; set; }
    }
}