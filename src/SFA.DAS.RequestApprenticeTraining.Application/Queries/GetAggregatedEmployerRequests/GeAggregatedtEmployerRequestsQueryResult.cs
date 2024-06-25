using System.Collections.Generic;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryResult
    {
        public List<AggregatedEmployerRequest> AggregatedEmployerRequests { get; set; }
    }
}
