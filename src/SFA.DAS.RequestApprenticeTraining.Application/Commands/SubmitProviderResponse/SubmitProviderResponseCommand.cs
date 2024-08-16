using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse

{
    public class SubmitProviderResponseCommand : IRequest<SubmitProviderResponseCommandResponse>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}