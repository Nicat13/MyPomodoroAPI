using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyPomodoro.Application.DTOs.Email;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Infrastructure.Persistence.Settings;

namespace MyPomodoro.Infrastructure.Persistence.Services
{
    public class EmailService : IEmailService
    {
        public APIAppSettings _apiSettings;

        public EmailService(IOptions<APIAppSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(request.From ?? _apiSettings.MailSettings.EmailFrom, _apiSettings.MailSettings.DisplayName);
                mail.To.Add(request.To);
                mail.Subject = request.Subject;
                var htmlView = AlternateView.CreateAlternateViewFromString(request.Body, null, "text/html");
                mail.IsBodyHtml = true;
                mail.AlternateViews.Add(htmlView);
                using (var smtpClient = new SmtpClient(_apiSettings.MailSettings.SmtpHost, _apiSettings.MailSettings.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_apiSettings.MailSettings.SmtpUser, _apiSettings.MailSettings.SmtpPass);
                    smtpClient.EnableSsl = _apiSettings.MailSettings.SSL;
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
                throw new HttpStatusException(new List<string> { e.Message });
            }
        }
    }
}