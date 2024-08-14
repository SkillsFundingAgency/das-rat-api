using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregeatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.AggregatedEmployerRequest
{
    public class WhenGettingAggregatedEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            GetAggregatedEmployerRequestsQueryResult aggregatedEmployerRequestResult,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aggregatedEmployerRequestResult);
            
            // Act
            var result = await controller.GetAggregatedEmployerRequests(12345);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(aggregatedEmployerRequestResult.AggregatedEmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (long accountId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetAggregatedEmployerRequests(12345);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
