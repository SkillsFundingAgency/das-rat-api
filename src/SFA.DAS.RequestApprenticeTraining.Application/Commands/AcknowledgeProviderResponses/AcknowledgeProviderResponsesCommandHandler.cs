using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommandHandler : IRequestHandler<AcknowledgeProviderResponsesCommand>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AcknowledgeProviderResponsesCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            IDateTimeProvider dateTimeProvider)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task Handle(AcknowledgeProviderResponsesCommand request, CancellationToken cancellationToken)
        {
            var employerRequest = await _employerRequestEntityContext.Get(request.EmployerRequestId);
            if (employerRequest != null && employerRequest.RequestStatus == RequestStatus.Active)
            {
                foreach (var response in employerRequest.ProviderResponseEmployerRequests)
                {
                    if (response.ProviderResponse != null && response.AcknowledgedAt == null && response.AcknowledgedBy == null)
                    {
                        response.AcknowledgedAt = _dateTimeProvider.Now;
                        response.AcknowledgedBy = request.AcknowledgedBy;
                    }
                }

                await _employerRequestEntityContext.SaveChangesAsync();
            }
        }
    }
}