using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.RespondToEmployerRequests

{
    public class RespondToEmployerRequestsCommand : IRequest<RespondToEmployerRequestsCommandResponse>
    {
        public long Ukprn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
    }
}