using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQuery : IRequest<GetSelectEmployerRequestsQueryResult>
    {
        public string StandardReference { get; set; }
        public string Ukprn { get; set; }
    }
}
