using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.CancelEmployerRequest
{
    [TestFixture]
    public class CancelEmployerRequestCommandHandlerTests
    {
        private Mock<IEmployerRequestEntityContext> _employerRequestEntityContextMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private Mock<ILogger<CancelEmployerRequestCommandHandler>> _loggerMock;
        private CancelEmployerRequestCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _employerRequestEntityContextMock = new Mock<IEmployerRequestEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _loggerMock = new Mock<ILogger<CancelEmployerRequestCommandHandler>>();

            _sut = new CancelEmployerRequestCommandHandler(
                _employerRequestEntityContextMock.Object,
                _dateTimeProviderMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCancelRequest_WhenEmployerRequestExists()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var command = new CancelEmployerRequestCommand { EmployerRequestId = Guid.NewGuid(), CancelledBy = Guid.NewGuid() };

            var employerRequest = new EmployerRequest
            {
                Id = command.EmployerRequestId,
                RequestStatus = RequestStatus.Active
            };

            _dateTimeProviderMock
                .Setup(p => p.Now)
                .Returns(now);

            _employerRequestEntityContextMock.Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync(employerRequest);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            employerRequest.RequestStatus.Should().Be(RequestStatus.Cancelled);
            employerRequest.CancelledAt.Should().Be(now);
            employerRequest.ModifiedBy.Should().Be(command.CancelledBy);
        }

        [Test]
        public async Task Handle_ShouldNotCancelEmployerRequest_WhenEmployerRequestDoesNotExist()
        {
            // Arrange
            var command = new CancelEmployerRequestCommand { EmployerRequestId = Guid.NewGuid(), CancelledBy = Guid.NewGuid() };

            _employerRequestEntityContextMock.Setup(x => x.Get(command.EmployerRequestId))
                .ReturnsAsync((EmployerRequest)null);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
