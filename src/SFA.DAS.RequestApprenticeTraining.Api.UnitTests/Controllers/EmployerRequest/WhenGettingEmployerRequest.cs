using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenGettingEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            GetEmployerRequestQueryResult employerRequestResult,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetEmployerRequestQuery>(t => t.EmployerRequestId == employerRequestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employerRequestResult);
            
            // Act
            var result = await controller.GetEmployerRequest(employerRequestId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(employerRequestResult.EmployerRequest);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerRequestQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetEmployerRequest(employerRequestId);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
