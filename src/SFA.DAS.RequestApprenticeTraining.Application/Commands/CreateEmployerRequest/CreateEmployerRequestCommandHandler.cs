using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommandHandler : IRequestHandler<CreateEmployerRequestCommand, CreateEmployerRequestCommandResponse>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly ILogger<CreateEmployerRequestCommandHandler> _logger;

        public CreateEmployerRequestCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            ILogger<CreateEmployerRequestCommandHandler> logger)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _logger = logger;
        }

        public async Task<CreateEmployerRequestCommandResponse> Handle(CreateEmployerRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Creating EmployerRequest record");

            var employerRequest = new EmployerRequest()
            {
                RequestType = request.RequestType
            };

            _employerRequestEntityContext.Add(employerRequest);
            await _employerRequestEntityContext.SaveChangesAsync();

            _logger.LogDebug($"Successfully created employer request record with Id: {employerRequest.Id}");
            return new CreateEmployerRequestCommandResponse() { EmployerRequestId = employerRequest.Id };
        }
    }
}