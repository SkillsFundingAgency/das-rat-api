using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
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
            var providerResponse = new ProviderResponse()
            {
                Email = request.Email,
                PhoneNumber = request.Phone,
                Website = request.Website,
                RespondedAt = DateTime.UtcNow,
                RespondedBy = request.RespondedBy,
            };

            _providerResponseEntityContext.Add(providerResponse);
            await _providerResponseEntityContext.SaveChangesAsync();

            var entitiesForUpdate = await _providerResponseEmployerRequestEntityContext.GetForProviderAndEmployerRequest(request.Ukprn, request.EmployerRequestIds);

            entitiesForUpdate.ForEach(entity => entity.ProviderResponseId = providerResponse.Id);

            await _providerResponseEmployerRequestEntityContext.SaveChangesAsync();

            return new SubmitProviderResponseCommandResponse { ProviderResponseId = providerResponse.Id };

        }
    }
}