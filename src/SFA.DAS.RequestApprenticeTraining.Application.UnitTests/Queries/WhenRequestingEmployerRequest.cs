using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingEmployerRequest
    {
        [Test, AutoMoqData]
        public async Task And_EmployerRequest_IsFound_ThenEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new EmployerRequest { Id = employerRequestId, RequestTypeId = 1 });
            context.SaveChanges();

            var query = new GetEmployerRequestQuery() { EmployerRequestId = employerRequestId };
            var handler = new GetEmployerRequestQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerRequestQueryResult 
            { 
                EmployerRequest = new Domain.Models.EmployerRequest { Id = employerRequestId, RequestTypeId = 1 } 
            };

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
