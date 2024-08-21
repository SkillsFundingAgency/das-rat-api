using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenHandlingGetEmployerAggregatedEmployerRequests
    {
        [Test, AutoMoqData]
        public async Task And_IsOnlyOneStandard_ThenSingleAggregatedEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            
            context.SaveChanges();

            var applicationSettings = Options.Create(new ApplicationSettings { ExpiryAfterMonths = 3 });

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 12345 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings);
             
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

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_IsMoreThanOneStandard_ThenMultipleAggregatedEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });

            context.SaveChanges();

            var applicationSettings = Options.Create(new ApplicationSettings { ExpiryAfterMonths = 3 });

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 12345 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings);

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
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId2,
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
        public async Task And_IsMoreThanOneStandard_ThenOnlyGivenAccountMultipleAggregatedEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var employerRequestId3 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId3, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 23451, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });

            context.SaveChanges();

            var applicationSettings = Options.Create(new ApplicationSettings { ExpiryAfterMonths = 3 });

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 12345 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings);

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
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId2,
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
        public async Task And_IsMoreThanOneStandard_AndAllProvidersViewedButNotResponded_ThenMultipleAggregatedEmployerRequestsAreReturnedWithNoResponses(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId1, Ukprn = 34512, ProviderResponseId = null });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 45123, ProviderResponseId = null });

            context.SaveChanges();

            var applicationSettings = Options.Create(new ApplicationSettings { ExpiryAfterMonths = 3 });

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 12345 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings);

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
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId2,
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
        public async Task And_IsMoreThanOneStandard_AndSomeProvidersResponded_ThenMultipleAggregatedEmployerRequestsAreReturnedWithSomeResponses(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var employerRequestId3 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard3 = new Standard { StandardReference = "ST0003", StandardTitle = "Standard 3", StandardLevel = 1, StandardSector = "Sector 1" };

            var provider45123RespondsToEmployerRequest2 = new ProviderResponse { Id = Guid.NewGuid(), AcknowledgedAt = DateTime.Now };
            var provider51234RespondsToEmployerRequest2 = new ProviderResponse { Id = Guid.NewGuid(), AcknowledgedAt = null };
            var provider45123RespondsToEmployerRequest3 = new ProviderResponse { Id = Guid.NewGuid(), AcknowledgedAt = null };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);
            context.Add(standard3);
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard1.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard2.StandardReference });
            context.Add(new EmployerRequest { Id = employerRequestId3, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard3.StandardReference });

            context.Add(provider45123RespondsToEmployerRequest2);
            context.Add(provider51234RespondsToEmployerRequest2);
            context.Add(provider45123RespondsToEmployerRequest3);

            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 45123, ProviderResponseId = provider45123RespondsToEmployerRequest2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 51234, ProviderResponseId = provider51234RespondsToEmployerRequest2.Id });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId2, Ukprn = 55112, ProviderResponseId = null });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequestId3, Ukprn = 45123, ProviderResponseId = provider45123RespondsToEmployerRequest3.Id });

            context.SaveChanges();

            var applicationSettings = Options.Create(new ApplicationSettings { ExpiryAfterMonths = 3 });

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 12345 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings);

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
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId2,
                        StandardReference = standard2.StandardReference,
                        StandardTitle = standard2.StandardTitle,
                        StandardLevel = standard2.StandardLevel,
                        RequestStatus = Domain.Models.Enums.RequestStatus.Active,
                        RequestedAt = requestedAt,
                        ExpiryAt = requestedAt.AddMonths(applicationSettings.Value.ExpiryAfterMonths),
                        NumberOfResponses = 2,
                        NewNumberOfResponses = 1
                    },
                    new Domain.Models.EmployerAggregatedEmployerRequest
                    {
                        EmployerRequestId = employerRequestId3,
                        StandardReference = standard3.StandardReference,
                        StandardTitle = standard3.StandardTitle,
                        StandardLevel = standard3.StandardLevel,
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
        public async Task Then_CancelledRequestsAreExcluded_FromAggregatedResults(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId1 = Guid.NewGuid();
            var employerRequestId2 = Guid.NewGuid();
            var requestedAt = DateTime.UtcNow.Date;

            var standard1 = new Standard { StandardReference = "ST0001", StandardTitle = "Standard 1", StandardLevel = 1, StandardSector = "Sector 1" };
            var standard2 = new Standard { StandardReference = "ST0002", StandardTitle = "Standard 2", StandardLevel = 1, StandardSector = "Sector 1" };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(standard1);
            context.Add(standard2);
            context.Add(new EmployerRequest { Id = employerRequestId1, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard1.StandardReference, RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            context.Add(new EmployerRequest { Id = employerRequestId2, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 12345, RequestedAt = requestedAt, StandardReference = standard2.StandardReference, RequestStatus = Domain.Models.Enums.RequestStatus.Cancelled });

            context.SaveChanges();

            var applicationSettings = Options.Create(new ApplicationSettings { ExpiryAfterMonths = 3 });

            var query = new GetEmployerAggregatedEmployerRequestsQuery { AccountId = 12345 };
            var handler = new GetEmployerAggregatedEmployerRequestsQueryHandler(context, applicationSettings);

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
    }
}
