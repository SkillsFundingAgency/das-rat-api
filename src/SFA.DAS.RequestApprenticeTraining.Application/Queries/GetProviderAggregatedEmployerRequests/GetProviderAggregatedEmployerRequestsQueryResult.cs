using System.Collections.Generic;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests
{
    public class GetProviderAggregatedEmployerRequestsQueryResult
    {
        public List<ProviderAggregatedEmployerRequest> ProviderAggregatedEmployerRequests { get; set; }
    }
}
