using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQuery : IRequest<GetEmployerRequestsByIdsQueryResult>
    {
        public List<Guid> EmployerRequestIds;

        public GetEmployerRequestsByIdsQuery(List<Guid> employerRequestIds) 
        {
            EmployerRequestIds = employerRequestIds;
        }
    }
}
