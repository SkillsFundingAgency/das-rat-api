﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingEmployerRequest
    {
        [Test, AutoMoqData]
        public async Task And_EmployerRequest_IsFound_ById_ThenEmployerRequestIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var employerRequestId = Guid.NewGuid();

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new EmployerRequest { Id = employerRequestId, RequestType = Domain.Models.Enums.RequestType.Shortlist });
            await context.SaveChangesAsync();

            var query = new GetEmployerRequestQuery() { EmployerRequestId = employerRequestId };
            var handler = new GetEmployerRequestQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetEmployerRequestQueryResult
            {
                EmployerRequest = new Domain.Models.EmployerRequest
                {
                    Id = employerRequestId,
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
            var query = new GetEmployerRequestQuery() { EmployerRequestId = Guid.NewGuid() };
            var handler = new GetEmployerRequestQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.EmployerRequest.Should().BeNull();
        }
    }
}
