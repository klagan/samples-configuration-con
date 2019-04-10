using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Samples.Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Dictionary<string, string>
            {
                ["HelloMessage"] = "Hello, World!",
                ["GoodbyeMessage"] = "Goodbye, World!"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("ConfigurationBasics.");

            var config = builder.Build();

            Console.WriteLine($"{config["HelloMessage"]}");
            Console.WriteLine($"{config["GoodbyeMessage"]}");

            Console.ReadKey();
        }
    }
}
