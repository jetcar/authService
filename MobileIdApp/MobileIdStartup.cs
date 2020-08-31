using AutoMapper;
using AutoMapper.Configuration;
using Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MobileId.MobileId;
using MobileIdApp.Controllers;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MobileIdApp
{
    public class MobileIdStartup
    {
        public MobileIdStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            var config = new MapperConfiguration(new MobileIdMapperProfile());
            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
            services.AddScoped<SessionDto>();
            services.AddScoped<IMobileIdService, MobileIdService>();

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddMvc()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Map("/mobileId",
                delegate (IApplicationBuilder mappedApp) { Configure1(mappedApp, env, "/mobileId"); });
        }

        public static void Configure1(IApplicationBuilder app, IWebHostEnvironment env, PathString virtualDirectory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UsePathBase(virtualDirectory);
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

    public class MobileIdMapperProfile : MapperConfigurationExpression
    {
    }
}