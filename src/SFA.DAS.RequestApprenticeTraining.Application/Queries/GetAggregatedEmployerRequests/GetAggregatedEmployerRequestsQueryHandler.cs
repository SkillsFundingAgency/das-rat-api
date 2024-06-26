using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetAggregatedEmployerRequestsQuery, GetAggregatedEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetAggregatedEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetAggregatedEmployerRequestsQueryResult> Handle(GetAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedRequests = await _employerRequestEntityContext.GetAggregatedEmployerRequests();

            return new GetAggregatedEmployerRequestsQueryResult
            {
                AggregatedEmployerRequests = 
                    aggregatedRequests.Select(entity => (Domain.Models.AggregatedEmployerRequest)entity).ToList()
            };
        }
    }
}
