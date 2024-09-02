using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.UnitTests.Application.Commands.CreateProviderResponseEmployerRequest
{
    [TestFixture]
    public class CreateProviderResponseEmployerRequestCommandHandlerTests
    {
        private Mock<IProviderResponseEmployerRequestEntityContext> _providerResponseEntityContextMock;
        private Mock<ILogger<AcknowledgeEmployerRequestsCommandHandler>> _loggerMock;
        private AcknowledgeEmployerRequestsCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _providerResponseEntityContextMock = new Mock<IProviderResponseEmployerRequestEntityContext>();
            _loggerMock = new Mock<ILogger<AcknowledgeEmployerRequestsCommandHandler>>();

            _sut = new AcknowledgeEmployerRequestsCommandHandler(
                _providerResponseEntityContextMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateProviderResponseEmployerRequest()
        {
            // Arrange
            var command = new AcknowledgeEmployerRequestsCommand
            {
                Ukprn = 456789456,
                EmployerRequestIds = new List<Guid> { new Guid() }
            };

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _providerResponseEntityContextMock.Verify(x => x.CreateIfNotExistsAsync(It.IsAny<ProviderResponseEmployerRequest>()), Times.Once);
            _providerResponseEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldCreateMultipleProviderResponseEmployerRequest()
        {
            // Arrange
            var command = new AcknowledgeEmployerRequestsCommand
            {
                Ukprn = 89745613,
                EmployerRequestIds = new List<Guid> { new Guid(), new Guid(), new Guid() }
            };

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _providerResponseEntityContextMock.Verify(x => x.CreateIfNotExistsAsync(It.IsAny<ProviderResponseEmployerRequest>()), Times.AtMost(3));
            _providerResponseEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
