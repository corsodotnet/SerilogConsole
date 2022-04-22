using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace SerilogConsole
{
    internal class Program
    {

        static void Main(string[] args)
        {
            IConfigurationBuilder builder = null;
            IHost host = null;

            try
            {
                builder = new ConfigurationBuilder();
                BuildConfing(builder);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    // .WriteTo.Seq("http://localhost:8081")
                    .CreateLogger();

                Log.Logger.Information("Application starting up....");
                host = (IHost)Hostbuilder();

                var myServices = ActivatorUtilities
                      .CreateInstance<BrunoServices>(host.Services);
                myServices.Run();

            }
            catch (Exception ex)
            {

                Log.Fatal("Application FAILED to Starting up!");
            }
            finally
            {
                // Log.CloseAndFlush();

            }
        }
        static void BuildConfing(IConfigurationBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var parameter = (!string.IsNullOrEmpty(env)) ? string.Concat(env, ".") : string.Empty;

            builder.SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"appsettings.{parameter}json", optional: false, reloadOnChange: true);
        }
        static IHost Hostbuilder()
        {
            return Host.CreateDefaultBuilder()
                 .ConfigureServices((context, services) =>
                 {
                     services.AddTransient<IBrunoServices, BrunoServices>();
                 })
                 .UseSerilog()
                 .Build();
        }
    }
}
