using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Sample.Configuration
{
    public class Person
    {
        public Name Name { get; set; }

        public int Age { get; set; }
    }

    public class Name
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class TestAppConfigCast
    {
        public void Execute(IConfiguration config)
        {
            var person = new Person();

            config.GetSection("Person").Bind(person);

            var result = config.GetSection("Person").Get<Person>();
        }
    }

    class Program
    {
        private static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.2

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            new TestAppConfigCast().Execute(Configuration);

            var option1 = Configuration["myOption1"];

            Console.WriteLine($"result: {option1}");

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
                .ConfigureAppConfiguration((context, mybuilder) =>
                {
                    var env = context.HostingEnvironment;
                    mybuilder.SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                        // Override config by env, using like Logging:Level or Logging__Level
                        .AddEnvironmentVariables();

                })
                .ConfigureServices((hostContext, services) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        // Development service configuration
                    }
                    else
                    {
                        // Non-development service configuration
                    }

                    //services.AddHostedService<LifetimeEventsHostedService>();
                    //services.AddHostedService<TimedHostedService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    //configLogging.AddConsole();
                    //configLogging.AddDebug();
                });

            using (var host = hostBuilder.Build())
            {
                host.Run();
            }
        }
    }
}
