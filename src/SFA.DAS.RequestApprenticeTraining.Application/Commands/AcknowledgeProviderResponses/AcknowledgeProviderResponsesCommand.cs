using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommand : IRequest
    {
        public Guid EmployerRequestId { get; set; }
        public Guid AcknowledgedBy { get; set;  }

        public static implicit operator AcknowledgeProviderResponsesCommand(AcknowledgeProviderResponsesRequest source)
        {
            return new AcknowledgeProviderResponsesCommand
            {
                AcknowledgedBy = source.AcknowledgedBy
            };
        }
    }
}