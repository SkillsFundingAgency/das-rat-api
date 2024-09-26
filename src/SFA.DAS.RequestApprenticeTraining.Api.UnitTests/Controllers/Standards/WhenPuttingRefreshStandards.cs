using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.RefreshStandards;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.Standards
{
    public class WhenPuttingRefreshStandards
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (RefreshStandardsParameters param,
            [Greedy] StandardsController controller)
        {
            // Act
            var result = await controller.RefreshStandards(param);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (RefreshStandardsParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] StandardsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<RefreshStandardsCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.RefreshStandards(param);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
