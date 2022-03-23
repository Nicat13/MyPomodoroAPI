using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.WebApi.Services;

namespace MyPomodoro.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceExtension(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MyPomodoro.WebApi",
                    Description = "This Api will be responsible for overall data distribution and authorization. Created by Nijat <3",
                    Contact = new OpenApiContact
                    {
                        Name = "nijat.net",
                        Email = "contact@nijat.net",
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }
    }
}