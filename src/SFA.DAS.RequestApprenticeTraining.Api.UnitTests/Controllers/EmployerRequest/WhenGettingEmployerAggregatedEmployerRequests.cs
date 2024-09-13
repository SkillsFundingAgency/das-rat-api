using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.AggregatedEmployerRequest
{
    public class WhenGettingEmployerAggregatedEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            GetEmployerAggregatedEmployerRequestsQueryResult employerAggregatedEmployerRequestResult,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employerAggregatedEmployerRequestResult);
            
            // Act
            var result = await controller.GetEmployerAggregatedEmployerRequests(12345);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(employerAggregatedEmployerRequestResult.EmployerAggregatedEmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetEmployerAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetEmployerAggregatedEmployerRequests(12345);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
