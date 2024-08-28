using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenGettingProviderResponseConfirmation
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (SubmitProviderResponseCommand command,
            [Frozen] Mock<IMediator> mediator,
            SubmitProviderResponseCommandResponse response,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange 
            mediator
                .Setup(m => m.Send(It.IsAny<SubmitProviderResponseCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await controller.SubmitProviderResponse(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(response);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (SubmitProviderResponseCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<SubmitProviderResponseCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.SubmitProviderResponse(command);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
