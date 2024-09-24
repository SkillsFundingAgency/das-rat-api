using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingStandard
    {
        [Test, AutoMoqData]
        public async Task And_Standard_IsFound_ByReference_ThenStandardIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            Standard standard)
        {
            // Arrange

            context.Add(standard);
            await context.SaveChangesAsync();

            var query = new GetStandardQuery() { StandardReference = standard.StandardReference };
            var handler = new GetStandardQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standard.Should().BeEquivalentTo(standard);
        }

        [Test, AutoMoqData]
        public async Task And_Standard_IsNotFound_ThenNullIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var query = new GetStandardQuery() { StandardReference = "XXXXX" };
            var handler = new GetStandardQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standard.Should().BeNull();
        }
    }
}
