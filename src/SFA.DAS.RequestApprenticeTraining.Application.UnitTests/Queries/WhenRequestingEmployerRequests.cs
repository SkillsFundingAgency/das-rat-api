using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingEmployerRequests
    {
        [Test, AutoMoqData]
        public async Task And_EmployerRequest_IsFound_ThenEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequest1Id = Guid.NewGuid();
            var employerRequest2Id = Guid.NewGuid();
            var employerRequest3Id = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new EmployerRequest { Id = employerRequest1Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1 });
            context.Add(new EmployerRequest { Id = employerRequest2Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1 });
            context.Add(new EmployerRequest { Id = employerRequest3Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 2 });
            context.SaveChanges();

            var query = new GetEmployerRequestsQuery() { AccountId = 1 };
            var handler = new GetEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerRequestsQueryResult 
            { 
                EmployerRequests = new List<Domain.Models.EmployerRequest>
                {
                    new Domain.Models.EmployerRequest { Id = employerRequest1Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1 },
                    new Domain.Models.EmployerRequest { Id = employerRequest2Id, RequestType = Domain.Models.Enums.RequestType.Shortlist, AccountId = 1 }
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
