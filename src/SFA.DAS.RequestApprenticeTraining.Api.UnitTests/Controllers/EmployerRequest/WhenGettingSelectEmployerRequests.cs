using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.AggregatedEmployerRequest
{
    public class WhenGettingSelectEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            GetSelectEmployerRequestsQueryResult selectEmployerRequestResult,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(selectEmployerRequestResult);

            // Act
            var result = await controller.GetSelectEmployerRequests(1234560, "ST0004");

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(selectEmployerRequestResult.SelectEmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetSelectEmployerRequests(1234560, "ST0004");

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
