using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest
{
    public class GetActiveEmployerRequestQuery : IRequest<GetActiveEmployerRequestQueryResult>
    {
        public long? AccountId { get; set; }
        public string StandardReference { get; set; }
    }
}
