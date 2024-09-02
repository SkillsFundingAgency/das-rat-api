using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
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

            if (providerResponse == null)
            {
                return new GetProviderResponseConfirmationQueryResult();
            }
            else
            {
                return new GetProviderResponseConfirmationQueryResult
                {
                    Ukprn = providerResponse.ProviderResponseEmployerRequests?.FirstOrDefault()?.Ukprn ?? 0,
                    Email = providerResponse.Email,
                    Phone = providerResponse.PhoneNumber,
                    Website = providerResponse.Website,
                    EmployerRequests = GetEmployerRequests(requests)
                };
            }
        }

        private List<Domain.Models.EmployerRequestReviewModel> GetEmployerRequests(List<EmployerRequestReviewModel> requests)
        { 
            if(requests.Count > 0) 
            {
                return requests.Select(entity => (Domain.Models.EmployerRequestReviewModel)entity).ToList();
            }
            return new List<Domain.Models.EmployerRequestReviewModel>();
        }
    }
}
