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
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private Mock<ILogger<AcknowledgeProviderResponsesCommandHandler>> _loggerMock;
        private AcknowledgeProviderResponsesCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _employerRequestEntityContextMock = new Mock<IEmployerRequestEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _loggerMock = new Mock<ILogger<AcknowledgeProviderResponsesCommandHandler>>();

            _sut = new AcknowledgeProviderResponsesCommandHandler(
                _employerRequestEntityContextMock.Object,
                _dateTimeProviderMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldAcknowledgeProviderResponses_WhenEmployerRequestExists()
        {
            // Arrange
            var today = DateTime.UtcNow;
            var userOne = Guid.NewGuid();

            var command = new AcknowledgeProviderResponsesCommand { EmployerRequestId = Guid.NewGuid(), AcknowledgedBy = userOne };

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

            _dateTimeProviderMock
                .Setup(p => p.Now)
                .Returns(today);

            _employerRequestEntityContextMock
                .Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().Be(today);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(userOne);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().Be(today);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedBy.Should().Be(userOne);
        }

        [Test]
        public async Task Handle_ShouldNotAcknowledgeProviderResponses_WhenProviderHasNotResponded()
        {
            // Arrange
            var today = DateTime.UtcNow;
            var userOne = Guid.NewGuid();

            var command = new AcknowledgeProviderResponsesCommand { EmployerRequestId = Guid.NewGuid(), AcknowledgedBy = userOne };

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
                        ProviderResponse = null // The provider has seen the request but has not responded to it
                    }
                }
            };

            _dateTimeProviderMock
                .Setup(p => p.Now)
                .Returns(today);

            _employerRequestEntityContextMock
                .Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().Be(today);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(userOne);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().BeNull();
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedBy.Should().BeNull();
        }

        [Test]
        public async Task Handle_ShouldNotAcknowledgeProviderResponses_WhenProviderResponseHasAlreadyBeenAcknowledged()
        {
            // Arrange
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var userOne = Guid.NewGuid();
            
            var today = DateTime.UtcNow;
            var userTwo = Guid.NewGuid();

            var command = new AcknowledgeProviderResponsesCommand { EmployerRequestId = Guid.NewGuid(), AcknowledgedBy = userTwo };

            var employerRequest = new EmployerRequest
            {
                Id = command.EmployerRequestId,
                ProviderResponseEmployerRequests = new List<ProviderResponseEmployerRequest>
                {
                    new ProviderResponseEmployerRequest
                    {
                        Ukprn = 11111,
                        ProviderResponse = new ProviderResponse(),
                        AcknowledgedAt = yesterday,
                        AcknowledgedBy = userOne
                    },
                    new ProviderResponseEmployerRequest
                    {
                        Ukprn = 22222,
                        ProviderResponse = new ProviderResponse(),
                        AcknowledgedAt = null,
                        AcknowledgedBy = null
                    }
                }
            };

            _dateTimeProviderMock
                .Setup(p => p.Now)
                .Returns(today);

            _employerRequestEntityContextMock
                .Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().Be(yesterday);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(userOne);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().Be(today);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedBy.Should().Be(userTwo);
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
