using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LoggingService;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.SystemConsole.Themes;
using DataService;
using FunctionalService;

namespace Steps_Count_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var dpContext = services.GetRequiredService<DataProtectionKeysContext>();
                    var functionSvc = services.GetRequiredService<IFunctionalSvc>();
                    //var countrySvc = services.GetRequiredService<ICountrySvc>();

                    //DbContextInitializer.Initialize(dpContext, context, functionSvc, countrySvc).Wait();
                    DbContextInitializer.Initialize(dpContext, context, functionSvc).Wait();

                }
                catch (Exception ex)                
                {
                    Log.Error("An error occurred while seeding the database  {Error} {StackTrace} {InnerException} {Source}",
                     ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                     .Enrich.FromLogContext()
                     .Enrich.WithProperty("Application", "Steps_Count_App")
                     .Enrich.WithProperty("MachineName", Environment.MachineName)
                     .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                     .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                     .Enrich.WithProperty("Version", Environment.Version)
                     .Enrich.WithProperty("UserName", Environment.UserName)
                     .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
                     .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                     //.WriteTo.Console(theme: AnsiConsoleTheme.Code)
                     .WriteTo.Console(theme: CustomConsoleTheme.VisualStudioCustomColors)
                     .WriteTo.File(formatter: new CustomFormatter(), path: Path.Combine(hostingContext.HostingEnvironment.ContentRootPath + $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}", $"Steps_Count_App_Logs_{DateTime.Now:yyyyMMdd}.txt"))
                    .ReadFrom.Configuration(hostingContext.Configuration)); ;
                    webBuilder.UseStartup<Startup>();
                });
    }
}
