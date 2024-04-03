using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Api
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromXml("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Starting up host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not start host");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseNServiceBusContainer()
                .UseNLog();
    }
}
