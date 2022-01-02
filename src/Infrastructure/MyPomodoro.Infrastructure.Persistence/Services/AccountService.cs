using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyPomodoro.Application.DTOs.Account;
using MyPomodoro.Application.DTOs.Email;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Settings;
using Task = System.Threading.Tasks.Task;

namespace MyPomodoro.Infrastructure.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public APIAppSettings _apiSettings;
        public AccountService(UserManager<ApplicationUser> userManager, IEmailService emailService, IOptions<APIAppSettings> apiSettings)
        {
            _userManager = userManager;
            _emailService = emailService;
            _apiSettings = apiSettings.Value;
        }



        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.Name,
                LastName = request.Surname,
                UserName = request.Email
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new HttpStatusException(result.Errors.Select(e => e.Description).ToList());
            }
            await SendEmailVerification(user);
            return await Task.FromResult<string>("User successfully registered and email verification link sent.");
        }

        public async Task<string> SendEmailVerification(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "account/confirmemail/";
            var _enpointUri = new Uri(string.Concat($"{_apiSettings.ClientAppOrigin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "email", user.Email);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            var emailRequest = new EmailRequest()
            {
                Body = $"Please tap the <a target='_blank' href='{verificationUri}'>button</a> to confirm your email address.",
                To = user.Email,
                Subject = "Email Confirmation",
            };
            await _emailService.SendAsync(emailRequest);
            return await Task.FromResult("Email Sent.");
        }
        public async Task<string> ConfirmEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Task.FromResult($"Account Confirmed for {user.Email}.");
            }
            else
            {
                throw new HttpStatusException(new List<string> { $"An error occured while confirming {user.Email}." });
            }
        }
    }
}