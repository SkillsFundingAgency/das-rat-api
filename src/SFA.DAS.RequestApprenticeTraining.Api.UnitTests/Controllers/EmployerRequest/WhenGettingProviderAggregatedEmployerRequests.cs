using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.AggregatedEmployerRequest
{
    public class WhenGettingProviderAggregatedEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            GetProviderAggregatedEmployerRequestsQueryResult aggregatedEmployerRequestResult,
            [Greedy] ProvidersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetProviderAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aggregatedEmployerRequestResult);
            
            // Act
            var result = await controller.GetProviderAggregatedEmployerRequests(12345);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(aggregatedEmployerRequestResult.ProviderAggregatedEmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetProviderAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetProviderAggregatedEmployerRequests(12345);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
