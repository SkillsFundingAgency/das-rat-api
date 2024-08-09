using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQueryHandler : IRequestHandler<GetEmployerRequestsForProviderQuery, GetEmployerRequestsByIdsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetEmployerRequestsByIdsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetEmployerRequestsByIdsQueryResult> Handle(GetEmployerRequestsForProviderQuery request, CancellationToken cancellationToken)
        {
            var selectRequests = await _employerRequestEntityContext.GetByIds(request.EmployerRequestIds);

            return new GetEmployerRequestsByIdsQueryResult
            {
                EmployerRequests =
                    selectRequests.Select(entity => (Domain.Models.SelectEmployerRequest)entity).ToList()
            };
        }
    }
}
