using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.RequestApprenticeTraining.Api.TaskQueue;
using SFA.DAS.RequestApprenticeTraining.Application.Behaviours;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SubmitEmployerRequestCommand).Assembly));

            services.AddValidatorsFromAssemblyContaining<SubmitEmployerRequestCommand>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            
            services.AddScoped<IDateTimeProvider, UtcDateTimeProvider>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddScoped<IEmployerRequestEntityContext>(s => s.GetRequiredService<RequestApprenticeTrainingDataContext>());
            services.AddScoped<IEmployerRequestRegionEntityContext>(s => s.GetRequiredService<RequestApprenticeTrainingDataContext>());
            services.AddScoped<IRegionEntityContext>(s => s.GetRequiredService<RequestApprenticeTrainingDataContext>());
            services.AddScoped<IProviderResponseEntityContext>(s => s.GetRequiredService<RequestApprenticeTrainingDataContext>());
            services.AddScoped<IProviderResponseEmployerRequestEntityContext>(s => s.GetRequiredService<RequestApprenticeTrainingDataContext>());
        }
    }
}