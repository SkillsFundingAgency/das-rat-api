using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsForProviderQuery : IRequest<GetEmployerRequestsByIdsQueryResult>
    {
        public List<Guid> EmployerRequestIds;

        public GetEmployerRequestsForProviderQuery(List<Guid> employerRequestIds) 
        {
            EmployerRequestIds = employerRequestIds;
        }
    }
}
