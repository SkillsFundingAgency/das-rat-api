using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.Settings
{
    public class WhenGettingSettings
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            ([Frozen] Mock<IMediator> mediator,
            GetSettingsQueryResult settingsResult,
            [Greedy] SettingsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetSettingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(settingsResult);

            // Act
            var result = await controller.GetSettings();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(settingsResult);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            ([Frozen] Mock<IMediator> mediator,
            [Greedy] SettingsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetSettingsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetSettings();

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
