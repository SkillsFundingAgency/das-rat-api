﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.RequestApprenticeTraining.Api.TaskQueue;
using SFA.DAS.RequestApprenticeTraining.Application.Behaviours;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;

namespace SFA.DAS.RequestApprenticeTraining.Api.AppStart
{
    public static class AddServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEmployerRequestCommand).Assembly));
            services.AddScoped<IEmployerRequestEntityContext>(s => s.GetRequiredService<RequestApprenticeTrainingDataContext>());
            services.AddScoped<IDateTimeProvider, UtcDateTimeProvider>();
            services.AddValidatorsFromAssembly(typeof(Application.Queries.GetEmployerRequest.GetEmployerRequestQueryValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        }
    }
}