using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests
{
    public class GetEmployerAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetEmployerAggregatedEmployerRequestsQuery, GetEmployerAggregatedEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly ApplicationSettings _applicationSettings;

        public GetEmployerAggregatedEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext, IOptions<ApplicationSettings> options)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _applicationSettings = options.Value;
        }

        public async Task<GetEmployerAggregatedEmployerRequestsQueryResult> Handle(GetEmployerAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedRequests = await _employerRequestEntityContext.GetEmployerAggregatedEmployerRequests(request.AccountId);

            var result = new GetEmployerAggregatedEmployerRequestsQueryResult
            {
                EmployerAggregatedEmployerRequests = 
                    aggregatedRequests.Select(entity => (EmployerAggregatedEmployerRequest)entity).ToList()
            };

            result.EmployerAggregatedEmployerRequests.ForEach(x => x.ExpiryAt = x.RequestedAt.AddMonths(_applicationSettings.ExpiryAfterMonths));
            return result;
        }
    }
}
