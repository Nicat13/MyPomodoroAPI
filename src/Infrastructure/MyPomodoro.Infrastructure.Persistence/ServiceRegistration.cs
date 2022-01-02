using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Application.Interfaces.Services;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;
using MyPomodoro.Infrastructure.Persistence.Repositories;
using MyPomodoro.Infrastructure.Persistence.Services;
using MyPomodoro.Infrastructure.Persistence.Settings;
using MyPomodoro.Infrastructure.Persistence.UnitOfWork;

namespace MyPomodoro.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration["APIAppSettings:ConnectionString"],
            b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.Configure<APIAppSettings>(configuration.GetSection("APIAppSettings"));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDapper, DapperClass>();
            services.AddScoped<IUowContext, UnitOfWorkContext>();
        }
        #region APIRepositories
        public static void AddPersistenceApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITestRepo, TestRepo>();
        }
        #endregion
    }
}
