using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetRegions;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingRegions
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            ([Frozen] Mock<IMediator> mediator,
            GetRegionsQueryResult regionsResult,
            [Greedy] RegionsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(regionsResult);

            // Act
            var result = await controller.GetRegions();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(regionsResult.Regions);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            ([Frozen] Mock<IMediator> mediator,
            [Greedy] RegionsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetRegions();

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
