using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests

{
    public class CreateProviderResponseEmployerRequestsCommand : IRequest<CreateProviderResponseEmployerRequestsCommandResponse>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
    }
}