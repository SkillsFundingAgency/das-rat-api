using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenHandlingGetProviderAggregatedEmployerRequests
    {

        private Mock<IOptions<ApplicationSettings>> _mockOptions;
        private DateTime _insideAllowedDateRangeForContactedRequests;
        private DateTime _outsideAllowedDateRangeForContactedRequests;

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<ApplicationSettings>>();

            var config = new ApplicationSettings
            {
                ProviderRemovedAfterRequestedMonths = 12,
            };
            _mockOptions.Setup(o => o.Value).Returns(config);

            _insideAllowedDateRangeForContactedRequests = DateTime.Now.AddMonths(-config.ProviderRemovedAfterRequestedMonths).AddDays(1);
            _outsideAllowedDateRangeForContactedRequests = DateTime.Now.AddMonths(-config.ProviderRemovedAfterRequestedMonths).AddDays(-1);
        }

        [Test, AutoMoqData]
        public async Task And_AggregatedEmployerRequest_IsSameStandard_ThenSingleAggregatedEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequest1Id = Guid.NewGuid();
            var employerRequest2Id = Guid.NewGuid();

            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle= standard1Title, StandardLevel = 1, StandardSector = "Sector 1"});
            context.Add(new EmployerRequest { Id = employerRequest1Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1, StandardReference= standard1Reference, NumberOfApprentices = 2 });
            context.Add(new EmployerRequest { Id = employerRequest2Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 2 ,StandardReference = standard1Reference, NumberOfApprentices = 1});
            
            await context.SaveChangesAsync();

            var query = new GetProviderAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetProviderAggregatedEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetProviderAggregatedEmployerRequestsQueryResult 
            { 
                ProviderAggregatedEmployerRequests = new List<Domain.Models.ProviderAggregatedEmployerRequest>
                {
                    new Domain.Models.ProviderAggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = true }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_AggregatedEmployerRequest_IsDifferentStandards_ThenMultipleAggregatedEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequest1Id = Guid.NewGuid();
            var employerRequest2Id = Guid.NewGuid();
            var employerRequest3Id = Guid.NewGuid();

            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";
            var standard2Reference = "ST0002";
            var standard2Title = "Standard 2";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new EmployerRequest { Id = employerRequest1Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1, StandardReference = standard1Reference, NumberOfApprentices = 2 });
            context.Add(new EmployerRequest { Id = employerRequest2Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 2, StandardReference = standard1Reference, NumberOfApprentices = 1 });
            context.Add(new EmployerRequest { Id = employerRequest3Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 3, StandardReference = standard2Reference, NumberOfApprentices = 1 });
            
            await context.SaveChangesAsync();

            var query = new GetProviderAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetProviderAggregatedEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetProviderAggregatedEmployerRequestsQueryResult
            {
                ProviderAggregatedEmployerRequests = new List<Domain.Models.ProviderAggregatedEmployerRequest>
                {
                    new Domain.Models.ProviderAggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = true },
                    new Domain.Models.ProviderAggregatedEmployerRequest { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 1, NumberOfEmployers = 1, IsNew = true }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_AggregatedEmployerRequest_IsSameStandardAndAllViewed_ThenSingleNotNewAggregatedEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequest1Id = Guid.NewGuid();
            var employerRequest2Id = Guid.NewGuid();

            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new EmployerRequest { Id = employerRequest1Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1, StandardReference = standard1Reference, NumberOfApprentices = 2 });
            context.Add(new EmployerRequest { Id = employerRequest2Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 2, StandardReference = standard1Reference, NumberOfApprentices = 1 });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequest1Id, Ukprn = 12345 });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequest2Id, Ukprn = 12345 });

            await context.SaveChangesAsync();

            var query = new GetProviderAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetProviderAggregatedEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetProviderAggregatedEmployerRequestsQueryResult
            {
                ProviderAggregatedEmployerRequests = new List<Domain.Models.ProviderAggregatedEmployerRequest>
                {
                    new Domain.Models.ProviderAggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = false }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_AggregatedEmployerRequest_IsSameStandardAndOneViewed_ThenSingleNewAggregatedEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequest1Id = Guid.NewGuid();
            var employerRequest2Id = Guid.NewGuid();

            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new EmployerRequest { Id = employerRequest1Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1, StandardReference = standard1Reference, NumberOfApprentices = 2 });
            context.Add(new EmployerRequest { Id = employerRequest2Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 2, StandardReference = standard1Reference, NumberOfApprentices = 1 });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequest1Id, Ukprn = 12345 });

            await context.SaveChangesAsync();

            var query = new GetProviderAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetProviderAggregatedEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetProviderAggregatedEmployerRequestsQueryResult
            {
                ProviderAggregatedEmployerRequests = new List<Domain.Models.ProviderAggregatedEmployerRequest>
                {
                    new Domain.Models.ProviderAggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = true }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task Should_ReturnActiveNotRespondedRequests_AndRespondedRequestsWithinAllowedDateRange(
           [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequest1Id = Guid.NewGuid();
            var employerRequest2Id = Guid.NewGuid();
            var employerRequest3Id = Guid.NewGuid();

            var providerResponseid = Guid.NewGuid();

            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new EmployerRequest { Id = employerRequest1Id, StandardReference = standard1Reference, NumberOfApprentices = 3, RequestedAt = _insideAllowedDateRangeForContactedRequests });
            context.Add(new EmployerRequest { Id = employerRequest2Id, StandardReference = standard1Reference, NumberOfApprentices = 4, RequestedAt = _outsideAllowedDateRangeForContactedRequests });
            context.Add(new EmployerRequest { Id = employerRequest3Id, StandardReference = standard1Reference, NumberOfApprentices = 5, RequestedAt = _insideAllowedDateRangeForContactedRequests });
            context.Add(new ProviderResponse { Id = providerResponseid });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequest1Id, Ukprn = 12345, ProviderResponseId = providerResponseid });
            context.Add(new ProviderResponseEmployerRequest { EmployerRequestId = employerRequest2Id, Ukprn = 12345, ProviderResponseId = providerResponseid });

            await context.SaveChangesAsync();

            var query = new GetProviderAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetProviderAggregatedEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetProviderAggregatedEmployerRequestsQueryResult
            {
                ProviderAggregatedEmployerRequests = new List<Domain.Models.ProviderAggregatedEmployerRequest>
                {
                    new Domain.Models.ProviderAggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 8, NumberOfEmployers = 2, IsNew = true }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

    }
}
