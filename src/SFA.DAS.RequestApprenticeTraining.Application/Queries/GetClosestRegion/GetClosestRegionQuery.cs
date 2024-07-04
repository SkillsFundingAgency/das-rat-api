using MediatR;
namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion
{
    public class GetClosestRegionQuery : IRequest<GetClosestRegionQueryResult>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
