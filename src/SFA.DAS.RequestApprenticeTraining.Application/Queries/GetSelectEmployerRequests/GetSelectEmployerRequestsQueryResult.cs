using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQueryResult
    {
        public List<SelectEmployerRequest> SelectEmployerRequests { get; set; }
    }
}
