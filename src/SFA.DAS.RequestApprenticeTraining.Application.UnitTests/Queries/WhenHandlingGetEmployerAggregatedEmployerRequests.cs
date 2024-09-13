using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenHandlingGetEmployerAggregatedEmployerRequests
    {
        [Test, AutoMoqData]
        public async Task And_EmployerHasRequestedOneStandard_AndOneProvidersRespondedToMultipeRequests_ThenSingleAggregatedRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            [Frozen] IOptions<ApplicationSettings> applicationSettings,
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            mockDateTimeProvider
                .Setup(p => p.Now)
                .Returns(dateTimeNow);

            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var employerRequestId3 = Guid.NewGuid();
            var requestedAt = dateTimeNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            var provider11111RespondsToEmployerRequest1And2 = new ProviderResponse { Id = Guid.NewGuid() };
            var provider22222RespondsToEmployerRequest2 = new ProviderResponse { Id = Guid.NewGuid() };
            var provider22222RespondsToEmployerRequest3 = new ProviderResponse { Id = Guid.NewGuid() };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);

            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 11111, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 22222, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId3, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 22222, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });

            context.Add(provider11111RespondsToEmployerRequest1And2);
            context.Add(provider22222RespondsToEmployerRequest2);
            context.Add(provider22222RespondsToEmployerRequest3);

            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId1, Ukprn = 11111, ProviderResponseId = provider11111RespondsToEmployerRequest1And2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 11111, ProviderResponseId = provider11111RespondsToEmployerRequest1And2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 22222, ProviderResponseId = provider22222RespondsToEmployerRequest2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId3, Ukprn = 33333, ProviderResponseId = null });

            await context.SaveChangesAsync();

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 11111 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings, mockDateTimeProvider.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerAggregatedEmployerRequestsQueryResult
            {
                EmployerAggregatedEmployerRequests = new List<Domain.Models.EmployerAggregatedEmployerRequest>
                {
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId1,
                        StandardReference = standard1.StandardReference,
                        StandardTitle = standard1.StandardTitle,
                        StandardLevel = standard1.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 1,
                        NewNumberOfResponses = 1
                    }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_EmployerHasRequestedTwoStandards_AndTwoProvidersRespondedToMultipleRequests_ThenSingleAggregatedRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            [Frozen] IOptions<ApplicationSettings> applicationSettings,
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            mockDateTimeProvider
                .Setup(p => p.Now)
                .Returns(dateTimeNow);

            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var employerRequestId3 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            var provider11111RespondsToEmployerRequest1And2 = new ProviderResponse { Id = Guid.NewGuid() };
            var provider22222RespondsToEmployerRequest2 = new ProviderResponse { Id = Guid.NewGuid() };
            var provider22222RespondsToEmployerRequest3 = new ProviderResponse { Id = Guid.NewGuid() };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);

            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 11111, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 22222, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId3, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 22222, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });

            context.Add(provider11111RespondsToEmployerRequest1And2);
            context.Add(provider22222RespondsToEmployerRequest2);
            context.Add(provider22222RespondsToEmployerRequest3);

            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId1, Ukprn = 11111, ProviderResponseId = provider11111RespondsToEmployerRequest1And2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 11111, ProviderResponseId = provider11111RespondsToEmployerRequest1And2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 22222, ProviderResponseId = provider22222RespondsToEmployerRequest2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId3, Ukprn = 33333, ProviderResponseId = null });

            await context.SaveChangesAsync();

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 22222 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings, mockDateTimeProvider.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerAggregatedEmployerRequestsQueryResult
            {
                EmployerAggregatedEmployerRequests = new List<Domain.Models.EmployerAggregatedEmployerRequest>
                {
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId2,
                        StandardReference = standard1.StandardReference,
                        StandardTitle = standard1.StandardTitle,
                        StandardLevel = standard1.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 2,
                        NewNumberOfResponses = 2
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId3,
                        StandardReference = standard2.StandardReference,
                        StandardTitle = standard2.StandardTitle,
                        StandardLevel = standard2.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 0,
                        NewNumberOfResponses = 0
                    }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_EmployerHasRequestedTwoStandards_AndEmployerHasAcknowledgedOneResponse_ThenAcknowledgedResponsesAreNotNewResponses(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            [Frozen] IOptions<ApplicationSettings> applicationSettings,
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            mockDateTimeProvider
                .Setup(p => p.Now)
                .Returns(dateTimeNow);

            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var employerRequestId3 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            var provider11111RespondsToEmployerRequest1And2 = new ProviderResponse { Id = Guid.NewGuid() };
            var provider22222RespondsToEmployerRequest2 = new ProviderResponse { Id = Guid.NewGuid() };
            var provider22222RespondsToEmployerRequest3 = new ProviderResponse { Id = Guid.NewGuid() };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);

            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 11111, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 22222, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId3, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 22222, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });

            context.Add(provider11111RespondsToEmployerRequest1And2);
            context.Add(provider22222RespondsToEmployerRequest2);
            context.Add(provider22222RespondsToEmployerRequest3);

            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId1, Ukprn = 11111, ProviderResponseId = provider11111RespondsToEmployerRequest1And2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 11111, ProviderResponseId = provider11111RespondsToEmployerRequest1And2.Id, AcknowledgedAt = DateTime.UtcNow });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 22222, ProviderResponseId = provider22222RespondsToEmployerRequest2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId3, Ukprn = 33333, ProviderResponseId = null });

            await context.SaveChangesAsync();

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 22222 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings, mockDateTimeProvider.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerAggregatedEmployerRequestsQueryResult
            {
                EmployerAggregatedEmployerRequests = new List<Domain.Models.EmployerAggregatedEmployerRequest>
                {
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId2,
                        StandardReference = standard1.StandardReference,
                        StandardTitle = standard1.StandardTitle,
                        StandardLevel = standard1.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 2,
                        NewNumberOfResponses = 1
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId3,
                        StandardReference = standard2.StandardReference,
                        StandardTitle = standard2.StandardTitle,
                        StandardLevel = standard2.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 0,
                        NewNumberOfResponses = 0
                    }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task Then_CancelledRequestsAreExcluded_FromAggregatedResults(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            [Frozen] IOptions<ApplicationSettings> applicationSettings,
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            mockDateTimeProvider
                .Setup(p => p.Now)
                .Returns(dateTimeNow);

            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);

            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 11111, RequestedAt = requestedAt, StandardReference = standard1.StandardReference, RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 11111, RequestedAt = requestedAt, StandardReference = standard2.StandardReference, RequestStatus = Domain.Models.Enums.RequestStatus.Cancelled });

            await context.SaveChangesAsync();

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 11111 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings, mockDateTimeProvider.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerAggregatedEmployerRequestsQueryResult
            {
                EmployerAggregatedEmployerRequests = new List<Domain.Models.EmployerAggregatedEmployerRequest>
                {
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId1,
                        StandardReference = standard1.StandardReference,
                        StandardTitle = standard1.StandardTitle,
                        StandardLevel = standard1.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 0,
                        NewNumberOfResponses = 0
                    }
                }
            };

            result.EmployerAggregatedEmployerRequests.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [AutoMoqInlineAutoData(-1, 1)]
        [AutoMoqInlineAutoData(0, 0)]
        [AutoMoqInlineAutoData(1, 0)]
        public async Task Then_ExpiredRequestsAreExludedAfterRemovedAfterDate_FromAggregatedResults(
            int daysAfterExpiry, int resultCount,
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            [Frozen] IOptions<ApplicationSettings> applicationSettings,
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            mockDateTimeProvider
                .Setup(p => p.Now)
                .Returns(dateTimeNow.AddDays(daysAfterExpiry));

            applicationSettings.Value.EmployerRemovedAfterExpiryMonths = 3;

            var employerRequestId1 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            
            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 11111, RequestedAt = requestedAt, StandardReference = standard1.StandardReference, RequestStatus = Domain.Models.Enums.RequestStatus.Expired, ExpiredAt = dateTimeNow.Date.AddMonths(-3) });
            
            await context.SaveChangesAsync();

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 11111 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings, mockDateTimeProvider.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.EmployerAggregatedEmployerRequests.Should().HaveCount(resultCount);
        }
    }
}
