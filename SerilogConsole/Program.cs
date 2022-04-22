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
            var builder = new ConfigurationBuilder();
            BuildConfing(builder);
            var host = Hostbuilder();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            var myServices = ActivatorUtilities
            .CreateInstance<BrunoServices>(host.Services);
            myServices.Run();

            Log.Logger.Information("Application starting up....");
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
