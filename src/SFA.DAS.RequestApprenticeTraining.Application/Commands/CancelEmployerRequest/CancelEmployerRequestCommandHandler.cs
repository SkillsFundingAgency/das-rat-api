using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest
{
    public class CancelEmployerRequestCommandHandler : IRequestHandler<CancelEmployerRequestCommand>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CancelEmployerRequestCommandHandler> _logger;

        public CancelEmployerRequestCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            IDateTimeProvider dateTimeProvider,
            ILogger<CancelEmployerRequestCommandHandler> logger)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task Handle(CancelEmployerRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Cancelling employer request for EmployerRequestId: {EmployerRequestId}", request.EmployerRequestId);

            var employerRequest = await _employerRequestEntityContext.Get(request.EmployerRequestId);
            if(employerRequest != null && employerRequest.RequestStatus == RequestStatus.Active)
            {
                employerRequest.RequestStatus = RequestStatus.Cancelled;
                employerRequest.CancelledAt = _dateTimeProvider.Now;
                employerRequest.ModifiedBy = request.CancelledBy; 

                await _employerRequestEntityContext.SaveChangesAsync();

                _logger.LogDebug("Cancelled employer request for EmployerRequestId: {EmployerRequestId}", request.EmployerRequestId);
            }
        }
    }
}