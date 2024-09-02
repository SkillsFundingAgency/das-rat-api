using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQueryHandler : IRequestHandler<GetSelectEmployerRequestsQuery, GetSelectEmployerRequestsQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly ApplicationSettings _applicationSettings;

        public GetSelectEmployerRequestsQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext, IOptions<ApplicationSettings> options)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _applicationSettings = options.Value;
        }

        public async Task<GetSelectEmployerRequestsQueryResult> Handle(GetSelectEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var selectRequests = await _employerRequestEntityContext.GetForProviderStandard(request.Ukprn, request.StandardReference, _applicationSettings.ProviderRemovedAfterRequestedMonths);

            return new GetSelectEmployerRequestsQueryResult
            {
                SelectEmployerRequests =
                    selectRequests.Select(entity => (Domain.Models.SelectEmployerRequest)entity).ToList()
            };
        }
    }
}
