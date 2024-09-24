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
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.PostStandard;
using Microsoft.AspNetCore.Components.Forms;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.Standards
{
    public class WhenPostingStandard
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (PostStandardRequest param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] StandardsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<PostStandardCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PostStandardCommandResponse
                {
                    Standard = new Standard
                    {
                        StandardLevel = param.StandardLevel,
                        StandardReference = param.StandardReference,
                        StandardSector = param.StandardSector,
                        StandardTitle = param.StandardTitle
                    }
                });

            // Act
            var result = await controller.Post(param);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(param);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (PostStandardRequest param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] StandardsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<PostStandardCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.Post(param);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
