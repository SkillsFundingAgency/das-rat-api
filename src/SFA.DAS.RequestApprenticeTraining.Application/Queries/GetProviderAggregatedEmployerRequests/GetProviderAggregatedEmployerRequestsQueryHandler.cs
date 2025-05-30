using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
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
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ApplicationSettings _applicationSettings;

        public GetProviderAggregatedEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext, 
            IDateTimeProvider dateTimeProvider,
            IOptions<ApplicationSettings> options)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _dateTimeProvider = dateTimeProvider;
            _applicationSettings = options.Value;
        }

        public async Task<GetProviderAggregatedEmployerRequestsQueryResult> Handle(GetProviderAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedRequests = await _employerRequestEntityContext.GetProviderAggregatedEmployerRequests(request.Ukprn, _applicationSettings.ProviderRemovedAfterRequestedMonths, _dateTimeProvider.Now);

            return new GetProviderAggregatedEmployerRequestsQueryResult
            {
                ProviderAggregatedEmployerRequests =
                    aggregatedRequests.Select(entity => (ProviderAggregatedEmployerRequest)entity).ToList()
            };
        }
    }
}
