using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenAcknowledgingProviderResponses
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk(
            Guid employerRequestId,
            Guid acknowledgedBy,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<AcknowledgeProviderResponsesCommand>(cmd =>
                        cmd.EmployerRequestId == employerRequestId && cmd.AcknowledgedBy == acknowledgedBy),
                    It.IsAny<CancellationToken>()));

            // Act
            var result = await controller.AcknowledgeProviderResponses(employerRequestId, new AcknowledgeProviderResponsesRequest { AcknowledgedBy = acknowledgedBy });

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommand_ReceivesEmployerRequestId(
            Guid employerRequestId,
            Guid acknowledgedBy,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Act
            await controller.AcknowledgeProviderResponses(employerRequestId, new AcknowledgeProviderResponsesRequest { AcknowledgedBy = acknowledgedBy });

            // Assert
            mediator.Verify(m => m.Send(It.Is<AcknowledgeProviderResponsesCommand>(cmd =>
                cmd.EmployerRequestId == employerRequestId &&
                cmd.AcknowledgedBy == acknowledgedBy),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandFailsDueToValidation_Then_ReturnBadRequest(
            Guid employerRequestId,
            Guid acknowledgedBy,
            ValidationException validationException,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<AcknowledgeProviderResponsesCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            // Act
            var result = await controller.AcknowledgeProviderResponses(employerRequestId, new AcknowledgeProviderResponsesRequest { AcknowledgedBy = acknowledgedBy });

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandFailsDueToException_Then_ReturnBadRequest(
            Guid employerRequestId,
            Guid acknowledgedBy,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<AcknowledgeProviderResponsesCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await controller.AcknowledgeProviderResponses(employerRequestId, new AcknowledgeProviderResponsesRequest { AcknowledgedBy = acknowledgedBy });

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
