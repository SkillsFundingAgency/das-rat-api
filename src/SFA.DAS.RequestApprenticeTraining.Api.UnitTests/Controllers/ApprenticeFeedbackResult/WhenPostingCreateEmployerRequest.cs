using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Api.Controllers;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequest
{
    public class WhenPostingCreateEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (CreateEmployerRequestCommand request,
            [Greedy] EmployerRequestController controller)
        {
            var result = await controller.Post(request);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (CreateEmployerRequestCommand request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            mediator.Setup(m => m.Send(It.IsAny<CreateEmployerRequestCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await controller.Post(request);

            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
