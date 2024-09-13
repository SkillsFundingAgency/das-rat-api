using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenGettingEmployerRequestsForResponseNotification
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (List<Guid> requestIds,
            [Frozen] Mock<IMediator> mediator,
            GetEmployerRequestsForResponseNotificationQueryResult erResult,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerRequestsForResponseNotificationQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(erResult);

            // Act
            var result = await controller.GetEmployerRequestsForResponseNotifications();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(erResult.EmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (List<Guid> requestIds,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerRequestsForResponseNotificationQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetEmployerRequestsForResponseNotifications();

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
