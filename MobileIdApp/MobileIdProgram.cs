using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MobileIdApp
{
    public class MobileIdProgram
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                            logging.AddDebug();
                        })
                        .UseStartup<MobileIdStartup>()
                        .UseUrls("http://localhost:5001/");
                });
    }
}