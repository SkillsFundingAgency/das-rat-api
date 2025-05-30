using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests
{
    public class AcknowledgeEmployerRequestsCommandHandler : IRequestHandler<AcknowledgeEmployerRequestsCommand>
    {
        private readonly IProviderResponseEmployerRequestEntityContext _providerResponseEmployerRequestEntityContext;
        private readonly ILogger<AcknowledgeEmployerRequestsCommandHandler> _logger;

        public AcknowledgeEmployerRequestsCommandHandler(
            IProviderResponseEmployerRequestEntityContext providerResponseEmployerRequestEntityContext,
            ILogger<AcknowledgeEmployerRequestsCommandHandler> logger)
        {
            _providerResponseEmployerRequestEntityContext = providerResponseEmployerRequestEntityContext;
            _logger = logger;
        }

        public async Task Handle(AcknowledgeEmployerRequestsCommand request, CancellationToken cancellationToken)
        {
            foreach (var employerRequestId in request.EmployerRequestIds)
            {
                var response = new ProviderResponseEmployerRequest
                {
                    EmployerRequestId = employerRequestId,
                    Ukprn = request.Ukprn,
                };
                await _providerResponseEmployerRequestEntityContext.CreateIfNotExistsAsync(response);
            }

            await _providerResponseEmployerRequestEntityContext.SaveChangesAsync();
        }
    }
}