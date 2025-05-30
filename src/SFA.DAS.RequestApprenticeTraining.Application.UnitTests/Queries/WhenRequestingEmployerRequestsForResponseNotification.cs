using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingEmployerRequestsForResponseNotification
    {
        [Test, AutoMoqData]
        public async Task And_GetEmployerRequestsForResponseNotification_RequestsRespondedAndNotAcknowledged_ThenEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);

            var standard1Reference = "ST0010";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0020";
            var standard2Title = "Standard 2";

            var employer1AccountId = 10001;

            var requestedBy1 = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });

            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard1Reference, requestedBy1, true, false);
            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard2Reference, requestedBy1, true, false);

            await context.SaveChangesAsync();

            var query = new GetEmployerRequestsForResponseNotificationQuery();
            var handler = new GetEmployerRequestsForResponseNotificationQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.EmployerRequests.Should().HaveCount(1);
            result.EmployerRequests[0].Standards.Should().HaveCount(2);

        }

        [Test, AutoMoqData]
        public async Task And_GetEmployerRequestsForResponseNotification_RequestsRespondedAndAcknowledged_ThenEmployerRequestsAreNotReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);

            var standard1Reference = "ST0010";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0020";
            var standard2Title = "Standard 2";

            var employer1AccountId = 10001;

            var requestedBy1 = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });

            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard1Reference, requestedBy1, true, true);
            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard2Reference, requestedBy1, true, false);

            await context.SaveChangesAsync();

            var query = new GetEmployerRequestsForResponseNotificationQuery();
            var handler = new GetEmployerRequestsForResponseNotificationQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.EmployerRequests.Should().HaveCount(1);
            result.EmployerRequests[0].Standards.Should().HaveCount(1);

        }

        [Test, AutoMoqData]
        public async Task And_GetEmployerRequestsForResponseNotification_OneEmployer_MultipleUsersWithRespondedRequests_MultipleRequestsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);

            var standard1Reference = "ST0010";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0020";
            var standard2Title = "Standard 2";

            var employer1AccountId = 10001;

            var requestedBy1 = Guid.NewGuid();
            var requestedBy2 = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });

            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard1Reference, requestedBy1, true, false);
            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard2Reference, requestedBy2, true, false);

            await context.SaveChangesAsync();

            var query = new GetEmployerRequestsForResponseNotificationQuery();
            var handler = new GetEmployerRequestsForResponseNotificationQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.EmployerRequests.Should().HaveCount(2);
            result.EmployerRequests[0].Standards.Should().HaveCount(1);
            result.EmployerRequests[1].Standards.Should().HaveCount(1);

        }

        [Test, AutoMoqData]
        public async Task And_GetEmployerRequestsForResponseNotification_NoRequestsInDB_NoResultsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            //Arrange
            var query = new GetEmployerRequestsForResponseNotificationQuery();
            var handler = new GetEmployerRequestsForResponseNotificationQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.EmployerRequests.Should().BeEmpty();
            
        }

        [Test, AutoMoqData]
        public async Task And_GetEmployerRequestsForResponseNotification_NoRespondedRequests_NoResultsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);

            var standard1Reference = "ST0010";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0020";
            var standard2Title = "Standard 2";

            var employer1AccountId = 10001;

            var requestedBy1 = Guid.NewGuid();
            var requestedBy2 = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });

            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard1Reference, requestedBy1, false, false);
            AddEmployerRequest(context, dateTimeNow, employer1AccountId, standard2Reference, requestedBy2, false, false);

            await context.SaveChangesAsync();

            var query = new GetEmployerRequestsForResponseNotificationQuery();
            var handler = new GetEmployerRequestsForResponseNotificationQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.EmployerRequests.Should().BeEmpty();

        }

        private void AddEmployerRequest(RequestApprenticeTrainingDataContext context, DateTime dateTimeNow,
            long accountId, string standardReference, Guid requestedBy, bool providerResponded, bool employerAcknowledged)
        {
            var employerRequest = new EmployerRequest 
            {
                AccountId = accountId,
                StandardReference = standardReference,
                AtApprenticesWorkplace = true,
                BlockRelease = true,
                Id = Guid.NewGuid(),    
                NumberOfApprentices = new Random().Next(1,10),
                RequestedAt = dateTimeNow,
                RequestedBy = requestedBy,
                RequestType = Domain.Models.Enums.RequestType.Shortlist,
            };

            context.Add(employerRequest);

            var providerResponseEmployerRequest = new ProviderResponseEmployerRequest 
            {
                EmployerRequestId = employerRequest.Id,
                Ukprn = 123456,
            };

            context.Add(providerResponseEmployerRequest);

            if (providerResponded)
            {
                ProviderResponse providerResponse = new ProviderResponse
                {
                    ContactName = "abd",
                    Email = "email@email.com",
                    PhoneNumber = "1234",
                    Website = "www.here.com",
                    RespondedAt = dateTimeNow,
                    RespondedBy = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                };

                context.Add(providerResponse);

                providerResponseEmployerRequest.ProviderResponseId = providerResponse.Id;

                if (employerAcknowledged)
                {
                    providerResponseEmployerRequest.AcknowledgedAt = dateTimeNow;
                    providerResponseEmployerRequest.AcknowledgedBy = Guid.NewGuid();
                }
            }
        }
    }
}
