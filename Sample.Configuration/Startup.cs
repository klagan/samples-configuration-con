using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Configuration.Models.Settings;

namespace Sample.Configuration
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("myAppSettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"myAppSettings.{env.EnvironmentName}.json", optional: true)
                //.AddXmlFile("app.config")
                .Build();
        }

        private IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MySettings>(options => Configuration.GetSection("MySettings").Bind(options));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILoggerFactory), typeof(LoggerFactory)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
        }

        public void Configure(IApplicationBuilder app, IOptions<MySettings> s, ILogger<Startup> logger)
        {
            logger.LogInformation($"Id: {s.Value.Id} => Name: {s.Value.SubSettings.Name}");
        }
    }
}
