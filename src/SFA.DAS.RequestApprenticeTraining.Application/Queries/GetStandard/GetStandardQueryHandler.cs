using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardQueryResult>
    {
        private readonly IStandardEntityContext _standardEntityContext;

        public GetStandardQueryHandler(IStandardEntityContext standardEntityContext)
        {
            _standardEntityContext = standardEntityContext;
        }

        public async Task<GetStandardQueryResult> Handle(GetStandardQuery request, CancellationToken cancellationToken)
        {
            Standard standard = await _standardEntityContext.Get(request.StandardReference);
            return new GetStandardQueryResult
            {
                Standard = (Domain.Models.Standard)standard,
            };
        }
    }
}
