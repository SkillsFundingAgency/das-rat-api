using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQuery : IRequest<GetStandardQueryResult>
    {
        public string StandardReference { get; set; }
    }
}
