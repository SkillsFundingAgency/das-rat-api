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
        private AcknowledgeProviderResponsesCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _employerRequestEntityContextMock = new Mock<IEmployerRequestEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _sut = new AcknowledgeProviderResponsesCommandHandler(
                _employerRequestEntityContextMock.Object,
                _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task Handle_ShouldAcknowledgeProviderResponses_WhenEmployerRequestExists()
        {
            // Arrange
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);
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
                .Returns(dateTimeNow);

            _employerRequestEntityContextMock
                .Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().Be(dateTimeNow);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(userOne);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().Be(dateTimeNow);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedBy.Should().Be(userOne);
        }

        [Test]
        public async Task Handle_ShouldNotAcknowledgeProviderResponses_WhenProviderHasNotResponded()
        {
            // Arrange
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);
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
                .Returns(dateTimeNow);

            _employerRequestEntityContextMock
                .Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().Be(dateTimeNow);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(userOne);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().BeNull();
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedBy.Should().BeNull();
        }

        [Test]
        public async Task Handle_ShouldNotAcknowledgeProviderResponses_WhenProviderResponseHasAlreadyBeenAcknowledged()
        {
            // Arrange
            var dateTimeToday = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);
            var dateTimeYesterday = dateTimeToday.AddDays(-1);
            var userOne = Guid.NewGuid();
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
                        AcknowledgedAt = dateTimeYesterday,
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
                .Returns(dateTimeToday);

            _employerRequestEntityContextMock
                .Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedAt.Should().Be(dateTimeYesterday);
            employerRequest.ProviderResponseEmployerRequests[0].AcknowledgedBy.Should().Be(userOne);
            employerRequest.ProviderResponseEmployerRequests[1].AcknowledgedAt.Should().Be(dateTimeToday);
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
