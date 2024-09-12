using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests
{
    public class GetEmployerAggregatedEmployerRequestsQuery : IRequest<GetEmployerAggregatedEmployerRequestsQueryResult>
    {
        public long AccountId { get; set; }
    }
}
