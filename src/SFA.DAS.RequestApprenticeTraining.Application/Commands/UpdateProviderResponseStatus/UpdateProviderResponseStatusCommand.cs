using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus

{
    public class UpdateProviderResponseStatusCommand : IRequest<UpdateProviderResponseStatusCommandResponse>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
        public int ProviderResponseStatus { get; set; }
    }
}