using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommandHandler : IRequestHandler<AcknowledgeProviderResponsesCommand>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<AcknowledgeProviderResponsesCommandHandler> _logger;

        public AcknowledgeProviderResponsesCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            IDateTimeProvider dateTimeProvider,
            ILogger<AcknowledgeProviderResponsesCommandHandler> logger)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task Handle(AcknowledgeProviderResponsesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Acknowledging provider responses for EmployerRequestId: {EmployerRequestId}", request.EmployerRequestId);

            var employerRequest = await _employerRequestEntityContext.Get(request.EmployerRequestId);
            if(employerRequest != null && employerRequest.RequestStatus == RequestStatus.Active)
            {
                foreach (var response in employerRequest.ProviderResponseEmployerRequests)
                {
                    response.AcknowledgedAt = _dateTimeProvider.Now;
                    response.AcknowledgedBy = request.AcknowledgedBy;
                }

                await _employerRequestEntityContext.SaveChangesAsync();

                _logger.LogDebug("Acknowledged provider responses for EmployerRequestId: {EmployerRequestId}", request.EmployerRequestId);
            }
        }
    }
}