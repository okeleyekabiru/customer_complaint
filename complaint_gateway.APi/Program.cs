using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace complaint_gateway.APi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
         
            var builder = WebHost.CreateDefaultBuilder(args);

            builder.ConfigureServices(s => s.AddSingleton(builder))
                .ConfigureAppConfiguration(
                    ic => ic.AddJsonFile("configuration.json"))
                .UseStartup<Startup>();
            var host = builder.Build();
            return host;
        }

    }
}
