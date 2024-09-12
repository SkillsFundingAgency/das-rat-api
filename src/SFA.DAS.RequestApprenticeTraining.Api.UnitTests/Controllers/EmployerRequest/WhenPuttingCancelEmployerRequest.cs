using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenPuttingCancelEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk(
            Guid employerRequestId,
            Guid cancelledBy,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<CancelEmployerRequestCommand>(cmd =>
                        cmd.EmployerRequestId == employerRequestId && cmd.CancelledBy == cancelledBy),
                    It.IsAny<CancellationToken>()));

            // Act
            var result = await controller.CancelEmployerRequest(employerRequestId, new CancelEmployerRequestRequest { CancelledBy = cancelledBy });

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommand_ReceivesEmployerRequestId(
            Guid employerRequestId,
            Guid cancelledBy,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Act
            await controller.CancelEmployerRequest(employerRequestId, new CancelEmployerRequestRequest { CancelledBy = cancelledBy });

            // Assert
            mediator.Verify(m => m.Send(It.Is<CancelEmployerRequestCommand>(cmd =>
                cmd.EmployerRequestId == employerRequestId &&
                cmd.CancelledBy == cancelledBy),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandFailsDueToValidation_Then_ReturnBadRequest(
            Guid employerRequestId,
            Guid cancelledBy,
            ValidationException validationException,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<CancelEmployerRequestCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            // Act
            var result = await controller.CancelEmployerRequest(employerRequestId, new CancelEmployerRequestRequest { CancelledBy = cancelledBy });

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandFailsDueToException_Then_ReturnBadRequest(
            Guid employerRequestId,
            Guid cancelledBy,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<CancelEmployerRequestCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await controller.CancelEmployerRequest(employerRequestId, new CancelEmployerRequestRequest { CancelledBy = cancelledBy });

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
