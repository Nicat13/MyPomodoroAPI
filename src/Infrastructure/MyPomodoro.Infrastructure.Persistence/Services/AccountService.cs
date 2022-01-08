using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyPomodoro.Application.DTOs.Account;
using MyPomodoro.Application.DTOs.Email;
using MyPomodoro.Application.DTOs.JWT;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Helpers;
using System.Security.Cryptography;
using MyPomodoro.Infrastructure.Persistence.Settings;
using Task = System.Threading.Tasks.Task;
using System.Net;

namespace MyPomodoro.Infrastructure.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IDateTimeService _dateTimeService;
        private APIAppSettings _apiSettings;
        private readonly IdentityContext _context;
        public AccountService(UserManager<ApplicationUser> userManager, IEmailService emailService, IOptions<APIAppSettings> apiSettings,
                                IdentityContext context, SignInManager<ApplicationUser> signInManager, IDateTimeService dateTimeService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _apiSettings = apiSettings.Value;
            _context = context;
            _signInManager = signInManager;
            _dateTimeService = dateTimeService;
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
            var result = await _userManager.CreateAsync(user, request.Password);
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
            if (user == null)
            {
                throw new HttpStatusException(new List<string> { $"No Accounts Registered with {email}." }, HttpStatusCode.NotFound);
            }
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

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            try
            {
                var user = await _context.Users.Include(x => x.RefreshToken)
                                .Where(x => x.Email == request.Email)
                                .FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new HttpStatusException(new List<string> { "Invalid Credentials." }, HttpStatusCode.Unauthorized);
                }
                if (!user.EmailConfirmed)
                {
                    await SendEmailVerification(user);
                    throw new HttpStatusException(new List<string> { "Email verification link sent, Please verify your e-mail address." }, HttpStatusCode.Unauthorized);
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    throw new HttpStatusException(new List<string> { "Invalid Credentials." }, HttpStatusCode.Unauthorized);
                }
                DateTime userLocalTime = await _dateTimeService.GetLocalTime(ipAddress);
                AuthenticationResponse response = new AuthenticationResponse();
                response.Jwt = await GenerateJWToken(user, userLocalTime);
                response.Email = user.Email;
                response.Name = user.FirstName;

                if (user.RefreshToken == null || user.RefreshToken.IsExpired)
                {
                    var refreshToken = GenerateRefreshToken(ipAddress, userLocalTime);
                    user.RefreshToken = refreshToken;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    response.RefreshToken = new RefreshTokenDto(refreshToken.Token, refreshToken.Expires);
                }
                else
                {
                    response.RefreshToken = new RefreshTokenDto(user.RefreshToken.Token, user.RefreshToken.Expires);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new HttpStatusException(new List<string> { e.Message });
            }
        }



        private async Task<JwtTokenDto> GenerateJWToken(ApplicationUser user, DateTime localTime)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roleClaims = new List<Claim>();
            string ipAddress = IpHelper.GetIpAddress();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.JWTSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var expires = (localTime).AddMinutes(_apiSettings.JWTSettings.DurationInMinutes);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _apiSettings.JWTSettings.Issuer,
                audience: _apiSettings.JWTSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials);
            var tokentHandler = new JwtSecurityTokenHandler();
            string token = tokentHandler.WriteToken(jwtSecurityToken);
            return new JwtTokenDto(token, expires);
        }
        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        private RefreshToken GenerateRefreshToken(string ipAddress, DateTime localTime)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = localTime.AddMonths(6),
                Created = localTime,
                CreatedByIp = ipAddress
            };
        }
        public Task<JwtTokenDto> RevokeByRefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new HttpStatusException(new List<string> { $"No Accounts Registered with {request.Email}." }, HttpStatusCode.NotFound);
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "account/resetpassword/";
            var _enpointUri = new Uri(string.Concat($"{_apiSettings.ClientAppOrigin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "email", user.Email);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            var emailRequest = new EmailRequest()
            {
                Body = $"Tap the <a target='_blank' href='{verificationUri}'>button</a> to reset your password.",
                To = user.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
            return await Task.FromResult("Reset password email sent.");
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new HttpStatusException(new List<string> { $"No Accounts Registered with {request.Email}." }, HttpStatusCode.NotFound);
            }
            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
            if (result.Succeeded)
            {
                return await Task.FromResult("Password Resetted.");
            }
            throw new HttpStatusException(new List<string> { "Error occured while reseting the password." });
        }
    }
}