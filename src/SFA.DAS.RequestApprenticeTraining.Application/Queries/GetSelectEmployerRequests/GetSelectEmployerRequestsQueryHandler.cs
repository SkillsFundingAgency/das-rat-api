using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQueryHandler : IRequestHandler<GetSelectEmployerRequestsQuery, GetSelectEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetSelectEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetSelectEmployerRequestsQueryResult> Handle(GetSelectEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var selectRequests = await _employerRequestEntityContext.GetForProviderStandard(request.Ukprn, request.StandardReference);

            return new GetSelectEmployerRequestsQueryResult
            {
                SelectEmployerRequests =
                    selectRequests.Select(entity => (Domain.Models.SelectEmployerRequest)entity).ToList()
            };
        }
    }
}
