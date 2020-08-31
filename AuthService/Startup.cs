using System;
using System.Collections.Generic;
using System.Linq;
using AuthService.Controllers;
using AuthService.Security;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MobileIdApp;
using Repository.DbModel;
using Repository.Repositories;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace AuthService
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
            var config = new MapperConfiguration(new MapperProfile());
            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
            services.AddScoped<SessionDto>();

            services.AddScoped<AuthController>();
            services.AddScoped<SignUpController>();
            services.AddScoped<UserProfileController>();

            services.AddScoped<MyDbContext>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidatorActionFilter));
            })
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
            MobileIdStartup.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Map("", delegate (IApplicationBuilder mappedApp) { Configure1(mappedApp, env); })
                .Map("/mobileId", delegate (IApplicationBuilder mappedApp) { MobileIdStartup.Configure1(mappedApp, env, "/mobileId"); });
        }

        public void Configure1(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}