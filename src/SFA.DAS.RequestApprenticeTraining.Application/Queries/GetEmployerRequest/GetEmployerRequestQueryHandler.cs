using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQueryHandler : IRequestHandler<GetEmployerRequestQuery, GetEmployerRequestQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetEmployerRequestQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetEmployerRequestQueryResult> Handle(GetEmployerRequestQuery request, CancellationToken cancellationToken)
        {
            EmployerRequest employerRequest = null;
            if (request.EmployerRequestId.HasValue)
            {
                employerRequest = await _employerRequestEntityContext.GetWithRegions(request.EmployerRequestId.Value);
            }
            else if(request.AccountId.HasValue && !string.IsNullOrEmpty(request.StandardReference))
            {
                employerRequest = await _employerRequestEntityContext.GetWithRegions(request.AccountId.Value, request.StandardReference);
            }

            return new GetEmployerRequestQueryResult
            {
                EmployerRequest = (Domain.Models.EmployerRequest)employerRequest
            };
        }
    }
}
