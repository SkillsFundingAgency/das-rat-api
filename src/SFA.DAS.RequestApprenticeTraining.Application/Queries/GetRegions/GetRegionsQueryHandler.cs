using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsQueryResult>
    {
        private readonly IRegionEntityContext _regionEntityContext;

        public GetRegionsQueryHandler(IRegionEntityContext regionEntityContext)
        {
            _regionEntityContext = regionEntityContext;
        }

        public async Task<GetRegionsQueryResult> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var regions = await _regionEntityContext.GetAll();

            return new GetRegionsQueryResult
            {
                Regions = regions.Select(entity => (Domain.Models.Region)entity).ToList()
            };
        }
    }
}
