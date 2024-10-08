﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenPostingSubmitProviderResponse
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (SubmitProviderResponseParameters param,
            [Greedy] ProvidersController controller)
        {
            // Act
            var result = await controller.SubmitProviderResponse(123456, param);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors
            (SubmitProviderResponseParameters param,
            [Frozen] Mock<IMediator> mediator,
            ValidationException validationException,
            [Greedy] ProvidersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<SubmitProviderResponseCommand>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);
            
            // Act
            var result = await controller.SubmitProviderResponse(789456123, param);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (SubmitProviderResponseParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<SubmitProviderResponseCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.SubmitProviderResponse(123456789, param);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
