using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands
{
    public class WhenHandlingCreateEmployerRequestCommand
    {
        [Test, AutoMoqData()]
        public async Task Then_CreatesEmployerRequest(
           [Frozen(Matching.ExactType)] IDateTimeProvider dateTimeProvider,
           [Frozen(Matching.ExactType)] IOptions<ApplicationSettings> applicationSettings,
           [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
           CreateEmployerRequestCommand command,
           CreateEmployerRequestCommandHandler handler
           )
        {
            // example of how to change the ApplicationSettings
            applicationSettings.Value.NServiceBusLicense = "New Value";

            // example of how to change the current time
            if (dateTimeProvider is SpecifiedDateTimeProvider specifiedDateTimeProvider)
            {
                specifiedDateTimeProvider.Advance(TimeSpan.FromDays(1));
            }

            var result = await handler.Handle(command, CancellationToken.None);

            result.EmployerRequestId.Should().NotBeEmpty();

            var employerRequest = await context.EmployerRequests.FirstOrDefaultAsync(s => s.Id == result.EmployerRequestId);
            employerRequest.RequestType.Should().Be(command.RequestType);
        }
    }
}