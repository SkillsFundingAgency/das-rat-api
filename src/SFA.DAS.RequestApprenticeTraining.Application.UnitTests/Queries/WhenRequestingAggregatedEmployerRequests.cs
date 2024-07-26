using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingAggregatedEmployerRequests
    {
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
            
            context.SaveChanges();

            var query = new GetAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetAggregatedEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetAggregatedEmployerRequestsQueryResult 
            { 
                AggregatedEmployerRequests = new List<Domain.Models.AggregatedEmployerRequest>
                {
                    new Domain.Models.AggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = true }
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
            
            context.SaveChanges();

            var query = new GetAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetAggregatedEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetAggregatedEmployerRequestsQueryResult
            {
                AggregatedEmployerRequests = new List<Domain.Models.AggregatedEmployerRequest>
                {
                    new Domain.Models.AggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = true },
                    new Domain.Models.AggregatedEmployerRequest { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 1, NumberOfEmployers = 1, IsNew = true }
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
            context.Add(new ProviderResponseEmployerRequestStatus { EmployerRequestId = employerRequest1Id, Ukprn = 12345 });
            context.Add(new ProviderResponseEmployerRequestStatus { EmployerRequestId = employerRequest2Id, Ukprn = 12345 });

            context.SaveChanges();

            var query = new GetAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetAggregatedEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetAggregatedEmployerRequestsQueryResult
            {
                AggregatedEmployerRequests = new List<Domain.Models.AggregatedEmployerRequest>
                {
                    new Domain.Models.AggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = false }
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

            context.SaveChanges();

            var query = new GetAggregatedEmployerRequestsQuery { Ukprn = 12345 };
            var handler = new GetAggregatedEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetAggregatedEmployerRequestsQueryResult
            {
                AggregatedEmployerRequests = new List<Domain.Models.AggregatedEmployerRequest>
                {
                    new Domain.Models.AggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2, IsNew = true }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
