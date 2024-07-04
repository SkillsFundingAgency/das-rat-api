using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion
{
    public class GetClosestRegionQueryHandler : IRequestHandler<GetClosestRegionQuery, GetClosestRegionQueryResult>
    {
        private readonly IRegionEntityContext _regionEntityContext;

        public GetClosestRegionQueryHandler(IRegionEntityContext regionEntityContext)
        {
            _regionEntityContext = regionEntityContext;
        }

        public async Task<GetClosestRegionQueryResult> Handle(GetClosestRegionQuery request, CancellationToken cancellationToken)
        {
            var closestRegion = await _regionEntityContext.FindClosestRegion(request.Latitude, request.Longitude);

            return new GetClosestRegionQueryResult
            {
                Region = (Domain.Models.Region)closestRegion
            };
        }
    }
}
