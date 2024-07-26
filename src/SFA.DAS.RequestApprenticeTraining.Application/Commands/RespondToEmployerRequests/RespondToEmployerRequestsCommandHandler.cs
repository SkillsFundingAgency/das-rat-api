using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.RespondToEmployerRequests
{
    public class RespondToEmployerRequestsCommandHandler : IRequestHandler<RespondToEmployerRequestsCommand, RespondToEmployerRequestsCommandResponse>
    {
        private readonly IProviderResponseEntityContext _providerResponseEntityContext;
        private readonly IProviderResponseEmployerRequestEntityContext _providerResponseEmployerRequestEntityContext;
        private readonly ILogger<RespondToEmployerRequestsCommandHandler> _logger;

        public RespondToEmployerRequestsCommandHandler(
            IProviderResponseEntityContext providerResponseEntityContext,
            IProviderResponseEmployerRequestEntityContext providerResponseEmployerRequestEntityContext,
            ILogger<RespondToEmployerRequestsCommandHandler> logger)
        {
            _providerResponseEntityContext = providerResponseEntityContext;
            _providerResponseEmployerRequestEntityContext = providerResponseEmployerRequestEntityContext;
            _logger = logger;
        }

        public async Task<RespondToEmployerRequestsCommandResponse> Handle(RespondToEmployerRequestsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Updating Provider Responses to Employer Requests");

            try
            {
                ProviderResponse response = new ProviderResponse 
                { 
                    Email = "something",//request.Email,
                    PhoneNumber = "123", //request.PhoneNumber,
                    RespondedAt = DateTime.UtcNow,
                    Website = "website", //request.Website,
                };

                var providerResponse = _providerResponseEntityContext.Add(response);
                await _providerResponseEntityContext.SaveChangesAsync();

                foreach (var employerRequestId in request.EmployerRequestIds)
                {
                    _providerResponseEmployerRequestEntityContext.Add(new ProviderResponseEmployerRequest
                    {
                        EmployerRequestId = employerRequestId,
                        Ukprn = request.Ukprn,
                        ProviderResponseId = providerResponse.Entity.Id,
                    });
                }

                await _providerResponseEmployerRequestEntityContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                var x = ex.InnerException;
            }

            return new RespondToEmployerRequestsCommandResponse() { Result = true};
        }
    }
}