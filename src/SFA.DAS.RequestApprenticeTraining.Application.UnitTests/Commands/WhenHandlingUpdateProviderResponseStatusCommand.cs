using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.UnitTests.Application.Commands.UpdateProviderResponseStatus
{
    [TestFixture]
    public class UpdateProviderResponseStatusCommandHandlerTests
    {
        private Mock<IProviderResponseEmployerRequestStatusEntityContext> _providerResponseStatusEntityContextMock;
        private Mock<ILogger<UpdateProviderResponseStatusCommandHandler>> _loggerMock;
        private UpdateProviderResponseStatusCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _providerResponseStatusEntityContextMock = new Mock<IProviderResponseEmployerRequestStatusEntityContext>();
            _loggerMock = new Mock<ILogger<UpdateProviderResponseStatusCommandHandler>>();

            _sut = new UpdateProviderResponseStatusCommandHandler(
                _providerResponseStatusEntityContextMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateProviderResponseEmployerRequestStatus()
        {
            // Arrange
            var command = new UpdateProviderResponseStatusCommand
            {
                Ukprn = 456789456,
                ProviderResponseStatus = 1,
                EmployerRequestIds = new List<Guid> { new Guid() }
            };
            
            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _providerResponseStatusEntityContextMock.Verify(x => x.Add(It.IsAny<ProviderResponseEmployerRequestStatus>()), Times.Once);
            _providerResponseStatusEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldCreateMultipleProviderResponseEmployerRequestStatus()
        {
            // Arrange
            var command = new UpdateProviderResponseStatusCommand
            {
                Ukprn = 89745613,
                ProviderResponseStatus = 1,
                EmployerRequestIds = new List<Guid> { new Guid(), new Guid(), new Guid() }
            };

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _providerResponseStatusEntityContextMock.Verify(x => x.Add(It.IsAny<ProviderResponseEmployerRequestStatus>()), Times.AtMost(3));
            _providerResponseStatusEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
