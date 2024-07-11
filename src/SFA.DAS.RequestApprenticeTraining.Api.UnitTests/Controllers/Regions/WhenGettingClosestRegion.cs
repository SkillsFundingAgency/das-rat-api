using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingClosestRegion
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            ([Frozen] Mock<IMediator> mediator,
            GetClosestRegionQueryResult closestRegionResult,
            double latitude,
            double longitude,
            [Greedy] RegionsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetClosestRegionQuery>(q => q.Latitude == latitude && q.Longitude == longitude), It.IsAny<CancellationToken>()))
                .ReturnsAsync(closestRegionResult);

            // Act
            var result = await controller.GetClosestRegion(latitude, longitude);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(closestRegionResult.Region);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            ([Frozen] Mock<IMediator> mediator,
            double latitude,
            double longitude,
            [Greedy] RegionsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetClosestRegionQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetClosestRegion(latitude, longitude);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
