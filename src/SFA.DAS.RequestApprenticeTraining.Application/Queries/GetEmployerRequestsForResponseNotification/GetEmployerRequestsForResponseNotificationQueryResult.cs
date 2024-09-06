using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification
{
    public class GetEmployerRequestsForResponseNotificationQueryResult
    {
        public List<EmployerRequestForResponseNotification> EmployerRequests { get; set; }
    }
}
