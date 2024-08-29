﻿using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQueryHandler : IRequestHandler<GetProviderResponseConfirmationQuery, GetProviderResponseConfirmationQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly IProviderResponseEntityContext _providerResponseEntityContext;

        public GetProviderResponseConfirmationQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext,
            IProviderResponseEntityContext providerResponseEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _providerResponseEntityContext = providerResponseEntityContext;
        }

        public async Task<GetProviderResponseConfirmationQueryResult> Handle(GetProviderResponseConfirmationQuery request, CancellationToken cancellationToken)
        {
            var requests = await _employerRequestEntityContext.GetForProviderResponse(request.ProviderResponseId);
            var providerResponse = await _providerResponseEntityContext.Get(request.ProviderResponseId);

            return providerResponse == null?
                new GetProviderResponseConfirmationQueryResult()
                :new GetProviderResponseConfirmationQueryResult
                {
                    Ukprn = providerResponse.ProviderResponseEmployerRequests.FirstOrDefault().Ukprn,
                    Email = providerResponse.Email,
                    Phone = providerResponse.PhoneNumber,
                    Website = providerResponse.Website,
                    EmployerRequests = requests.Any()
                    ? requests.Select(entity => (Domain.Models.EmployerRequestReviewModel)entity).ToList()
                    : new List<Domain.Models.EmployerRequestReviewModel>()
                };
        }
    }
}
