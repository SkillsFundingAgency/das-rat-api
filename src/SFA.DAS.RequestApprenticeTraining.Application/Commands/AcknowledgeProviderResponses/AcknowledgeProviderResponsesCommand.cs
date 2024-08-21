using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommand : IRequest
    {
        public AcknowledgeProviderResponsesCommand(Guid employerRequestId, Guid acknowledgedBy)
        {
            EmployerRequestId = employerRequestId;
            AcknowledgedBy = acknowledgedBy;
        }

        public Guid EmployerRequestId { get; private set; }
        public Guid AcknowledgedBy { get; private set;  }
    }
}