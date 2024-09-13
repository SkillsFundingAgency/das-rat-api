using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenPostingCreateEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (long accountId,
            SubmitEmployerRequestRequest request,
            [Greedy] AccountsController controller)
        {
            // Act
            var result = await controller.SubmitEmployerRequest(accountId, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommand_ReceivesAccountId
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            SubmitEmployerRequestRequest request,
            [Greedy] AccountsController controller)
        {
            // Act
            await controller.SubmitEmployerRequest(accountId, request);

            // Assert
            mediator.Verify(m => m.Send(It.Is<SubmitEmployerRequestCommand>(cmd =>
                cmd.AccountId == accountId &&
                cmd.OriginalLocation == request.OriginalLocation &&
                cmd.RequestType == request.RequestType &&
                cmd.StandardReference == request.StandardReference &&
                cmd.NumberOfApprentices == request.NumberOfApprentices &&
                cmd.SameLocation == request.SameLocation &&
                cmd.SingleLocation == request.SingleLocation &&
                cmd.MultipleLocations.SequenceEqual(request.MultipleLocations) &&
                cmd.AtApprenticesWorkplace == request.AtApprenticesWorkplace &&
                cmd.DayRelease == request.DayRelease &&
                cmd.BlockRelease == request.BlockRelease &&
                cmd.RequestedBy == request.RequestedBy &&
                cmd.ModifiedBy == request.ModifiedBy),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors
            (long accountId,
            SubmitEmployerRequestRequest request,
            [Frozen] Mock<IMediator> mediator,
            ValidationException validationException,
            [Greedy] AccountsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<SubmitEmployerRequestCommand>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);
            
            // Act
            var result = await controller.SubmitEmployerRequest(accountId, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (long accountId, 
            SubmitEmployerRequestRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] AccountsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<SubmitEmployerRequestCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.SubmitEmployerRequest(accountId, request);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
