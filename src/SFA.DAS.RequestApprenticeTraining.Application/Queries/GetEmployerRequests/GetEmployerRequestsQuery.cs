using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsQuery : IRequest<GetEmployerRequestsQueryResult>
    {
        public long AccountId { get; set; }
    }
}
