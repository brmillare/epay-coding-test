using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using static RestServer.Controllers.CustomersController;

namespace MyApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}|{Level:u3}|{SourceProgram}|{SourceContext}|{Message:1j}{NewLine}{Exception}")
              .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting REST server...");
                //start the run of the REST server
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "REST server terminated unexpectedly.");
                Environment.Exit(1);
            }
            finally
            {
                Log.Information("REST server shutdown completed.");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //call Startup from CustomerController
                    webBuilder.UseStartup<Startup>();
                });
        }
    }

}

