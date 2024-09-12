using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest
{
    public class GetActiveEmployerRequestQueryHandler : IRequestHandler<GetActiveEmployerRequestQuery, GetActiveEmployerRequestQueryResult>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;

        public GetActiveEmployerRequestQueryHandler(IEmployerRequestEntityContext employerRequestEntityContext)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
        }

        public async Task<GetActiveEmployerRequestQueryResult> Handle(GetActiveEmployerRequestQuery request, CancellationToken cancellationToken)
        {
            EmployerRequest employerRequest = await _employerRequestEntityContext.GetActive(request.AccountId.Value, request.StandardReference);
            return new GetActiveEmployerRequestQueryResult
            {
                EmployerRequest = (Domain.Models.EmployerRequest)employerRequest
            };
        }
    }
}
