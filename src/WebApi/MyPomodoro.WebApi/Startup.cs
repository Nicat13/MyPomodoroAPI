using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyPomodoro.Application;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.WebApi.Extensions;


namespace MyPomodoro.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddIdentity<ApplicationUser, IdentityRole<string>>().AddRoles<IdentityRole<string>>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            services.AddControllers();
            services.AddApplicationLayer();
            services.AddPersistenceRegistration(Configuration);
            services.AddPersistenceApiServices(Configuration);
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseErrorHandling();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
