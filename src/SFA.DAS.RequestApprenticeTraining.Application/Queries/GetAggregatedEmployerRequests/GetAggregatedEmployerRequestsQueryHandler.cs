using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetAggregatedEmployerRequestsQuery, GetAggregatedEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly ApplicationSettings _applicationSettings;

        public GetAggregatedEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext, IOptions<ApplicationSettings> options)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _applicationSettings = options.Value;
        }

        public async Task<GetAggregatedEmployerRequestsQueryResult> Handle(GetAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedRequests = await _employerRequestEntityContext.GetAggregatedEmployerRequests(request.Ukprn, _applicationSettings.ProviderRemovedAfterRequestedMonths);

            return new GetAggregatedEmployerRequestsQueryResult
            {
                AggregatedEmployerRequests = 
                    aggregatedRequests.Select(entity => (AggregatedEmployerRequest)entity).ToList()
            };
        }
    }
}
