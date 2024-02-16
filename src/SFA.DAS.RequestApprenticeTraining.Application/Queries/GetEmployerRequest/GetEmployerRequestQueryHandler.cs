using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
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
            var employerRequest = await _employerRequestEntityContext.GetForEmployerRequestId(request.EmployerRequestId);

            return new GetEmployerRequestQueryResult
            {
                EmployerRequest = (Domain.Models.EmployerRequest)employerRequest
            };
        }
    }
}
