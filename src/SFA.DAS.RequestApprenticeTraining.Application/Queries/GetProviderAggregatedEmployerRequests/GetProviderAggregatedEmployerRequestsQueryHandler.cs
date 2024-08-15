using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests
{
    public class GetProviderAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetProviderAggregatedEmployerRequestsQuery, GetProviderAggregatedEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetProviderAggregatedEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetProviderAggregatedEmployerRequestsQueryResult> Handle(GetProviderAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedRequests = await _employerRequestEntityContext.GetProviderAggregatedEmployerRequests(request.Ukprn);

            return new GetProviderAggregatedEmployerRequestsQueryResult
            {
                ProviderAggregatedEmployerRequests = 
                    aggregatedRequests.Select(entity => (ProviderAggregatedEmployerRequest)entity).ToList()
            };
        }
    }
}
