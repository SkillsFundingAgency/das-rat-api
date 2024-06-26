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
        public async Task And_AggregatedEmployerRequest_IsFound_ThenAggregatedEmployerRequestIsReturned(
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

            var query = new GetAggregatedEmployerRequestsQuery();
            var handler = new GetAggregatedEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetAggregatedEmployerRequestsQueryResult 
            { 
                AggregatedEmployerRequests = new List<Domain.Models.AggregatedEmployerRequest>
                {
                    new Domain.Models.AggregatedEmployerRequest { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1", NumberOfApprentices = 3, NumberOfEmployers = 2 }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
