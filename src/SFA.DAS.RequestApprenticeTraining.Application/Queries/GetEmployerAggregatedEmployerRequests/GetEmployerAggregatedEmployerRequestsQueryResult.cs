using System.Collections.Generic;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests
{
    public class GetEmployerAggregatedEmployerRequestsQueryResult
    {
        public List<EmployerAggregatedEmployerRequest> EmployerAggregatedEmployerRequests { get; set; }
    }
}
