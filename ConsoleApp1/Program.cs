using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    public class MySettings
    {
        public MySettings()
        {
            SubSettings = new SubSettings();
        }

        public string Id { get; set; }

        public SubSettings SubSettings { get; set; }
    }

    public class SubSettings
    {
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var inMemory = new Dictionary<string, string>
            {
                {"site", "https://g0t4.github.io/pluralsight-dotnet-core-xplat-apps"},
                {"output:folder", "reports"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemory)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("myAppSettings.json", true)
                .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray())
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .Build();

            var l = host.Services.GetService<ILogger<Program>>();
            l.LogInformation("kam testing");
            l.LogCritical("something critical");
            l.LogDebug("something debug");
            l.LogError("something error");
            l.LogTrace("something trace");
            l.LogWarning("something warning");

            host.Run();
        }
    }
}
