﻿using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommandHandler : IRequestHandler<AcknowledgeProviderResponsesCommand>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        
        
        private readonly ILogger<AcknowledgeProviderResponsesCommandHandler> _logger;

        public AcknowledgeProviderResponsesCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            ILogger<AcknowledgeProviderResponsesCommandHandler> logger)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _logger = logger;
        }

        public async Task Handle(AcknowledgeProviderResponsesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Acknowledging provider responses for EmployerRequestId: {request.EmployerRequestId}");

            var employerRequest = await _employerRequestEntityContext.GetWithResponses(request.EmployerRequestId);
            if(employerRequest != null)
            {
                var responses = employerRequest.ProviderResponseEmployerRequests
                    .Select(prer => prer.ProviderResponse)
                    .Where(pr => pr != null);

                foreach (var response in responses)
                {
                    response.AcknowledgedAt = DateTime.UtcNow;
                    response.AcknowledgedBy = request.AcknowledgedBy;
                }

                await _employerRequestEntityContext.SaveChangesAsync();
            }

            _logger.LogDebug($"Acknowledged provider responses for EmployerRequestId: {request.EmployerRequestId}");
        }
    }
}