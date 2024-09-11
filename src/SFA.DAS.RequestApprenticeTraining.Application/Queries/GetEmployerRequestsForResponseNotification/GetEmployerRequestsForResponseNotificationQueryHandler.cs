using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification
{
    public class GetEmployerRequestsForResponseNotificationQueryHandler : IRequestHandler<GetEmployerRequestsForResponseNotificationQuery, GetEmployerRequestsForResponseNotificationQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetEmployerRequestsForResponseNotificationQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetEmployerRequestsForResponseNotificationQueryResult> Handle(GetEmployerRequestsForResponseNotificationQuery query, CancellationToken cancellationToken)
        {

            var requests = await _employerRequestEntityContext.GetForResponseNotification();

            return new GetEmployerRequestsForResponseNotificationQueryResult
            {
                EmployerRequests =
                    requests.Select(entity => (EmployerRequestForResponseNotification)entity).ToList()
            };
        }
    }
}
