using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQueryResult
    {
        public List<SelectEmployerRequest> EmployerRequests { get; set; }
    }
}
