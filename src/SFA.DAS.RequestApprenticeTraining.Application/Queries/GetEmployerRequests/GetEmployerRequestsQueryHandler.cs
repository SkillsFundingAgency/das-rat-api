using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsQueryHandler : IRequestHandler<GetEmployerRequestsQuery, GetEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetEmployerRequestsQueryResult> Handle(GetEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var employerRequests = await _employerRequestEntityContext.Get(request.AccountId);

            return new GetEmployerRequestsQueryResult
            {
                EmployerRequests = employerRequests.Select(entity => (Domain.Models.EmployerRequest)entity).ToList()
            };
        }
    }
}
