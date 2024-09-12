using MediatR;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQuery : IRequest<GetSelectEmployerRequestsQueryResult>
    {
        public string StandardReference { get; set; }
        public long Ukprn { get; set; }

        public GetSelectEmployerRequestsQuery(long ukprn, string standardReference) 
        {
            StandardReference = standardReference;
            Ukprn = ukprn;
        }
    }
}
