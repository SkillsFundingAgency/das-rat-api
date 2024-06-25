using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQuery : IRequest<GetAggregatedEmployerRequestsQueryResult>
    {
    }
}
