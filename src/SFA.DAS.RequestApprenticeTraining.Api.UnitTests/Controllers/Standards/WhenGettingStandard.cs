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
using Microsoft.AspNetCore.Components.Forms;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetStandard;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.Standards
{
    public class WhenGettingStandard
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (string standardReference,
            GetStandardQueryResult standardResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] StandardsController controller)
        {   
            //Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetStandardQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(standardResult);

            // Act
            var result = await controller.Get(standardReference);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(standardResult.Standard);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (string standardReference,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] StandardsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetStandardQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.Get(standardReference);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
