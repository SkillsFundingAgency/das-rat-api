using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingActiveEmployerRequest
    {
        [Test, AutoMoqData]
        public async Task And_EmployerRequest_IsFound_ByAccountIdAndStandardReference_ThenEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId = Guid.NewGuid();
            var accountId = 123;
            var standardReference = "ST0013";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standardReference });
            context.Add(new EmployerRequest { Id = employerRequestId, AccountId = accountId, StandardReference = standardReference, RequestType = Domain.Models.Enums.RequestType.Shortlist });
            await context.SaveChangesAsync();

            var query = new GetActiveEmployerRequestQuery() { AccountId = accountId, StandardReference = standardReference };
            var handler = new GetActiveEmployerRequestQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetActiveEmployerRequestQueryResult
            {
                EmployerRequest = new Domain.Models.EmployerRequest
                {
                    Id = employerRequestId,
                    AccountId = accountId,
                    StandardReference = standardReference,
                    RequestType = Domain.Models.Enums.RequestType.Shortlist,
                    Regions = new List<Domain.Models.Region>(),
                    ProviderResponses = new List<Domain.Models.ProviderResponse>()
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_EmployerRequest_IsNotFound_ThenNullIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var query = new GetActiveEmployerRequestQuery() { AccountId = 9999, StandardReference = "ST0099" };
            var handler = new GetActiveEmployerRequestQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.EmployerRequest.Should().BeNull();
        }
    }
}
