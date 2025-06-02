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
    public class ExpireEmployerRequestsCommandHandler : IRequestHandler<ExpireEmployerRequestsCommand>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ApplicationSettings _applicationSettings;

        public ExpireEmployerRequestsCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            IDateTimeProvider dateTimeProvider,
            IOptions<ApplicationSettings> options)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _dateTimeProvider = dateTimeProvider;
            _applicationSettings = options.Value;
        }

        public async Task Handle(ExpireEmployerRequestsCommand request, CancellationToken cancellationToken)
        {
            await _employerRequestEntityContext.ExpireEmployerRequests(_applicationSettings.ExpiryAfterMonths, _dateTimeProvider.Now);
            await _employerRequestEntityContext.SaveChangesAsync();
        }
    }
}