using System.Collections.Generic;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsQueryResult
    {
        public List<EmployerRequest> EmployerRequests { get; set; }
    }
}
