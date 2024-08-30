using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.ExpireEmployerRequests
{
    public class ExpireEmployerRequestsCommandHandler : IRequestHandler<ExpireEmployerRequestsCommand, ExpireEmployerRequestsCommandResponse>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly ILogger<ExpireEmployerRequestsCommandHandler> _logger;
        private readonly ApplicationSettings _applicationSettings;

        public ExpireEmployerRequestsCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            ILogger<ExpireEmployerRequestsCommandHandler> logger,
            IOptions<ApplicationSettings> options)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _logger = logger;
            _applicationSettings = options.Value;
        }

        public async Task<ExpireEmployerRequestsCommandResponse> Handle(ExpireEmployerRequestsCommand request, CancellationToken cancellationToken)
        {
            await _employerRequestEntityContext.ExpireEmployerRequests(_applicationSettings.ExpiryAfterMonths);
            await _employerRequestEntityContext.SaveChangesAsync();

            return new ExpireEmployerRequestsCommandResponse();
        }
    }
}