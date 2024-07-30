using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenPostingCreateproviderResponseEmpoyerRequests
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (CreateProviderResponseEmployerRequestsCommand request,
            [Greedy] EmployerRequestController controller)
        {
            // Act
            var result = await controller.CreateProviderResponses(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors
            (CreateProviderResponseEmployerRequestsCommand request,
            [Frozen] Mock<IMediator> mediator,
            ValidationException validationException,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<CreateProviderResponseEmployerRequestsCommand>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);
            
            // Act
            var result = await controller.CreateProviderResponses(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (CreateProviderResponseEmployerRequestsCommand request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<CreateProviderResponseEmployerRequestsCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.CreateProviderResponses(request);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
