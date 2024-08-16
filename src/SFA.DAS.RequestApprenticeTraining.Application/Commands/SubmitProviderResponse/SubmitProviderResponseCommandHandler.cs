using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommandHandler : IRequestHandler<SubmitProviderResponseCommand, SubmitProviderResponseCommandResponse>
    {
        private readonly IProviderResponseEntityContext _providerResponseEntityContext;
        private readonly IProviderResponseEmployerRequestEntityContext _providerResponseEmployerRequestEntityContext;
        private readonly ILogger<SubmitProviderResponseCommandHandler> _logger;

        public SubmitProviderResponseCommandHandler(
            IProviderResponseEntityContext providerResponseEntityContext,
            IProviderResponseEmployerRequestEntityContext providerResponseEmployerRequestEntityContext,
            ILogger<SubmitProviderResponseCommandHandler> logger)
        {
            _providerResponseEntityContext = providerResponseEntityContext;
            _providerResponseEmployerRequestEntityContext = providerResponseEmployerRequestEntityContext;
            _logger = logger;
        }

        public async Task<SubmitProviderResponseCommandResponse> Handle(SubmitProviderResponseCommand request, CancellationToken cancellationToken)
        {
            var providerResponse = _providerResponseEntityContext.Add(
                new ProviderResponse()
                {
                    Email = request.Email,
                    PhoneNumber = request.Phone,
                    Website = request.Website,
                    RespondedAt = DateTime.UtcNow,
                });

            await _providerResponseEntityContext.SaveChangesAsync();

            var entitiesForUpdate = _providerResponseEmployerRequestEntityContext.Entities
                 .Where(e => e.Ukprn == request.Ukprn && request.EmployerRequestIds.Contains(e.EmployerRequestId))
                 .ToList();

            entitiesForUpdate.ForEach(entity => entity.ProviderResponseId = providerResponse.Entity.Id);

            await _providerResponseEmployerRequestEntityContext.SaveChangesAsync();

            return new SubmitProviderResponseCommandResponse { ProviderResponseId = providerResponse.Entity.Id };

        }
    }
}