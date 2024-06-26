﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public class RequestApprenticeTrainingHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultsDescription = "Request Apprentice Training API Health Check";
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public RequestApprenticeTrainingHealthCheck(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var dbConnectionHealthy = true;
            try
            {
                await _employerRequestEntityContext.GetFirstOrDefault();
            }
            catch
            {
                dbConnectionHealthy = false;
            }

            return dbConnectionHealthy ? HealthCheckResult.Healthy(HealthCheckResultsDescription) : HealthCheckResult.Unhealthy(HealthCheckResultsDescription);
        }
    }
}