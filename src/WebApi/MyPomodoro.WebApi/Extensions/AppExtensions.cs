using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using MyPomodoro.Application.Exceptions;
using System;


namespace MyPomodoro.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyPomodoro.WebApi");
            });
        }

        public static void UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var code = 400;
                string message = exception.Message;
                context.Response.ContentType = "text/*";
                if (exception is HttpStatusException httpException)
                {
                    code = (int)httpException.statusCode;
                    message = String.Join("\n", httpException.msgs);
                }
                context.Response.StatusCode = code;
                await context.Response.WriteAsync(message);
            }));
        }
    }
}
