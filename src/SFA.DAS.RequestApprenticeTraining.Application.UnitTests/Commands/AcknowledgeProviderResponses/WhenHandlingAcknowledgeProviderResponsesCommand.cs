using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.AcknowledgeProviderResponses
{
    [TestFixture]
    public class AcknowledgeProviderResponsesCommandHandlerTests
    {
        private Mock<IEmployerRequestEntityContext> _employerRequestEntityContextMock;
        private Mock<ILogger<AcknowledgeProviderResponsesCommandHandler>> _loggerMock;
        private AcknowledgeProviderResponsesCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _employerRequestEntityContextMock = new Mock<IEmployerRequestEntityContext>();
            _loggerMock = new Mock<ILogger<AcknowledgeProviderResponsesCommandHandler>>();

            _sut = new AcknowledgeProviderResponsesCommandHandler(
                _employerRequestEntityContextMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldAcknowledgeProviderResponses_WhenEmployerRequestExists()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand { EmployerRequestId = Guid.NewGuid(), AcknowledgedBy = Guid.NewGuid() };

            var employerRequest = new EmployerRequest
            {
                Id = command.EmployerRequestId,
                ProviderResponseEmployerRequests = new List<ProviderResponseEmployerRequest>
                {
                    new ProviderResponseEmployerRequest
                    {
                        Ukprn = 11111,
                        ProviderResponse = new ProviderResponse()
                    },
                    new ProviderResponseEmployerRequest
                    {
                        Ukprn = 22222,
                        ProviderResponse = new ProviderResponse()
                    }
                }
            };

            _employerRequestEntityContextMock.Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().NotBeNull();
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(command.AcknowledgedBy);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().NotBeNull();
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedBy.Should().Be(command.AcknowledgedBy);
        }

        [Test]
        public async Task Handle_ShouldNotAcknowledgeProviderResponses_WhenEmployerRequestDoesNotExist()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand { EmployerRequestId = Guid.NewGuid(), AcknowledgedBy = Guid.NewGuid() };

            _employerRequestEntityContextMock.Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync((EmployerRequest)null);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
