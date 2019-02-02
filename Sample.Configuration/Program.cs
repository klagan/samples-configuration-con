using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Configuration.Models.Settings;

namespace Sample.Configuration
{
    class Program
    {
        private static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            var inMemory = new Dictionary<string, string>
            {
                {"site", "https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.2"},
                {"output:folder", "reports"},
            };
            
            //build the configuration up
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemory)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                //enable this line to get the settings from the .confgi file rather than .json file
                //.AddXmlFile("app.config")
                .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray())
                .AddEnvironmentVariables()
                .Build();
            
            //this code block would set up an HTTP host - notice use of startup class
            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup<Startup>()
            //    .ConfigureLogging((hostingContext, logging) =>
            //    {
            //        //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //        logging.AddConsole();
            //        logging.AddDebug();
            //        logging.AddEventSourceLogger();
            //    })
            //    .Build();

            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    config.AddEnvironmentVariables();
                    
                    if (args != null)
                    {
                        // enviroment from command line
                        // e.g.: dotnet run --environment "Staging"
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        // do seomthing special
                    }

                    // read settings from configuration
                    services.Configure<MySettings>(options => Configuration.GetSection("MySettings").Bind(options));

                    services.TryAdd(ServiceDescriptor.Singleton(typeof(ILoggerFactory), typeof(LoggerFactory)));

                    services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
                })
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    //read logging configuration from configuration
                    //loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                    loggingBuilder.AddEventSourceLogger();
                });

            using (var host = hostBuilder.Build())
            {
                var l = host.Services.GetService<ILogger<Program>>();

                l.LogInformation("kam testing");
                l.LogCritical("something critical");
                l.LogDebug("something debug");
                l.LogError("something error");
                l.LogTrace("something trace");
                l.LogWarning("something warning");

                var option1 = Configuration["myOption1"];
                l.LogInformation($"myOption1: {option1}");

                var mySettings = host.Services.GetService<IOptions<MySettings>>().Value;
                l.LogInformation($"Id: {mySettings.Id} => Name: {mySettings.SubSettings.Name}");

                host.Run();
            }
        }
    }
}
