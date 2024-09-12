using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Api.TaskQueue;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using System;
using System.Net;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenPostingExpireEmployerRequests
    {
        private Mock<IBackgroundTaskQueue> _backgroundTaskQueue;
        private Mock<IMediator> _mediatorMock;
        private EmployerRequestController _sut;

        [SetUp]
        public void SetupTests()
        {
            _backgroundTaskQueue = new Mock<IBackgroundTaskQueue>();
            _mediatorMock = new Mock<IMediator>();
            _sut = new EmployerRequestController(_mediatorMock.Object, Mock.Of<ILogger<EmployerRequestController>>(), _backgroundTaskQueue.Object);
        }

        [Test]
        public void When_PostToExpireEmployerRequests_Then_BackgroundTaskIsQueued()
        {
            // Act
            var result = _sut.ExpireEmployerRequests();

            // Assert
            _backgroundTaskQueue.Verify(m => m.QueueBackgroundRequest(
               It.IsAny<ExpireEmployerRequestsCommand>(),
               "expire employer requests",
               It.IsAny<Action<object, TimeSpan, ILogger<TaskQueueHostedService>>>()),
               Times.Once);
        }

        [Test]
        public void When_PostToExpireEmployerRequestsHasNoErrors_Then_ReturnsAccepted()
        {
            // Act
            var controllerResult = _sut.ExpireEmployerRequests() as ObjectResult;

            // Assert

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        }
    }
}
