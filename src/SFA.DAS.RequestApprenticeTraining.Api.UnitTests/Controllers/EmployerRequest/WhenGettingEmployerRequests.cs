using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenGettingEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            GetEmployerRequestsQueryResult employerRequestResult,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetEmployerRequestsQuery>(t => t.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employerRequestResult);

            // Act
            var result = await controller.GetEmployerRequests(accountId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(employerRequestResult.EmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            ValidationException validationException,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);

            // Act
            var result = await controller.GetEmployerRequests(accountId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetEmployerRequests(accountId);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
