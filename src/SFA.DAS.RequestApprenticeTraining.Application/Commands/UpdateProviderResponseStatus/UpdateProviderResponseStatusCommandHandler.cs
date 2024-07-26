using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus
{
    public class UpdateProviderResponseStatusCommandHandler : IRequestHandler<UpdateProviderResponseStatusCommand, UpdateProviderResponseStatusCommandResponse>
    {
        private readonly IProviderResponseEmployerRequestStatusEntityContext _providerResponseEmployerRequestStatusEntityContext;
        private readonly ILogger<UpdateProviderResponseStatusCommandHandler> _logger;

        public UpdateProviderResponseStatusCommandHandler(
            IProviderResponseEmployerRequestStatusEntityContext providerResponseEmployerRequestEntityContext,
            ILogger<UpdateProviderResponseStatusCommandHandler> logger)
        {
            _providerResponseEmployerRequestStatusEntityContext = providerResponseEmployerRequestEntityContext;
            _logger = logger;
        }

        public async Task<UpdateProviderResponseStatusCommandResponse> Handle(UpdateProviderResponseStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Updating Provider Responses to Employer Requests");

            try
            {
                foreach (var employerRequestId in request.EmployerRequestIds)
                {
                    _providerResponseEmployerRequestStatusEntityContext.Add(new ProviderResponseEmployerRequestStatus 
                    {
                        EmployerRequestId = employerRequestId,
                        Ukprn = request.Ukprn,
                        ResponseStatus = request.ProviderResponseStatus
                    });
                }

                await _providerResponseEmployerRequestStatusEntityContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                var x = ex.InnerException;
            }

            return new UpdateProviderResponseStatusCommandResponse() { Result = true};
        }
    }
}