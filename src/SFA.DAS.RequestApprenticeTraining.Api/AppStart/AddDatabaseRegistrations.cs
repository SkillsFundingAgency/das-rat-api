using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Api.AppStart
{
    public static class AddDatabaseRegistrations
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
            if (configuration.IsLocalAcceptanceOrDev())
            {
                services.AddDbContext<RequestApprenticeTrainingDataContext>(options => options.UseSqlServer(appSettings.DbConnectionString).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            }
            else if (configuration.IsIntegrationTests())
            {
                services.AddDbContext<RequestApprenticeTrainingDataContext>(options => options.UseSqlServer("Server=localhost;Database=SFA.DAS.RequestApprenticeTraining.IntegrationTests.Database;Trusted_Connection=True;MultipleActiveResultSets=true").EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            }
            else
            {
                services.AddSingleton(new ChainedTokenCredential(
                    new ManagedIdentityCredential(),
                    new AzureCliCredential(),
                    new VisualStudioCodeCredential(),
                    new VisualStudioCredential())
            );
                services.AddDbContext<RequestApprenticeTrainingDataContext>(ServiceLifetime.Transient);
            }

            services.AddTransient(provider => new Lazy<RequestApprenticeTrainingDataContext>(provider.GetService<RequestApprenticeTrainingDataContext>()));
        }
    }
}
