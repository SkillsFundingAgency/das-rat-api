using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests
{
    public class CreateProviderResponseEmployerRequestsCommandHandler : IRequestHandler<CreateProviderResponseEmployerRequestsCommand, CreateProviderResponseEmployerRequestsCommandResponse>
    {
        private readonly IProviderResponseEmployerRequestEntityContext _providerResponseEmployerRequestEntityContext;
        private readonly ILogger<CreateProviderResponseEmployerRequestsCommandHandler> _logger;

        public CreateProviderResponseEmployerRequestsCommandHandler(
            IProviderResponseEmployerRequestEntityContext providerResponseEmployerRequestEntityContext,
            ILogger<CreateProviderResponseEmployerRequestsCommandHandler> logger)
        {
            _providerResponseEmployerRequestEntityContext = providerResponseEmployerRequestEntityContext;
            _logger = logger;
        }

        public async Task<CreateProviderResponseEmployerRequestsCommandResponse> Handle(CreateProviderResponseEmployerRequestsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Updating Provider Responses to Employer Requests");

            try
            {
                foreach (var employerRequestId in request.EmployerRequestIds)
                {
                    var response = new ProviderResponseEmployerRequest
                    {
                        EmployerRequestId = employerRequestId,
                        Ukprn = request.Ukprn,
                    };
                    _providerResponseEmployerRequestEntityContext.Add(response);
                }

                await _providerResponseEmployerRequestEntityContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                var x = ex.InnerException;
            }

            return new CreateProviderResponseEmployerRequestsCommandResponse() { Result = true};
        }
    }
}