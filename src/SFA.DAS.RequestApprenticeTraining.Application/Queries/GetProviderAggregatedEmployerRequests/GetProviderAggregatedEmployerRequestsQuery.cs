using MediatR;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests
{
    public class GetProviderAggregatedEmployerRequestsQuery : IRequest<GetProviderAggregatedEmployerRequestsQueryResult>
    {
        public long Ukprn { get; set; }
    }
}
