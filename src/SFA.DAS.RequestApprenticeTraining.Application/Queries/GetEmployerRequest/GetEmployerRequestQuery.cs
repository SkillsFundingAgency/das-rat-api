using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQuery : IRequest<GetEmployerRequestQueryResult>
    {
        public Guid? EmployerRequestId { get; set; }
    }
}
