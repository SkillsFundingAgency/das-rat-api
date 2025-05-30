using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.SubmitProviderResponse
{
    [TestFixture]
    public class SubmitProviderResponseCommandHandlerTests
    {
        private Mock<IProviderResponseEntityContext> _providerResponseEntityContextMock;
        private Mock<IProviderResponseEmployerRequestEntityContext> _providerResponseEmployerRequestEntityContextMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private SubmitProviderResponseCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.SetupGet(s => s.Now).Returns(dateTimeNow);

            _providerResponseEntityContextMock = new Mock<IProviderResponseEntityContext>();
            _providerResponseEmployerRequestEntityContextMock = new Mock<IProviderResponseEmployerRequestEntityContext>();

            _sut = new SubmitProviderResponseCommandHandler(
                _providerResponseEntityContextMock.Object,
                _providerResponseEmployerRequestEntityContextMock.Object,
                _dateTimeProviderMock.Object);
        }

        [Test, AutoMoqData]
        public async Task Handle_ShouldSubmitProviderResponse(
            SubmitProviderResponseCommand command,
            List<ProviderResponseEmployerRequest> mockEntities,
            long ukprn)
        {
            //Arrange
            mockEntities.ForEach(x =>
            {
                x.ProviderResponseId = null;
                x.ProviderResponse = null;
                x.Ukprn = ukprn;
            });

            _providerResponseEmployerRequestEntityContextMock.Setup(c => c.GetForProviderAndEmployerRequest(command.Ukprn, command.EmployerRequestIds))
                .ReturnsAsync(mockEntities);

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            _providerResponseEntityContextMock.Verify(x => x.Add(It.IsAny<ProviderResponse>()), Times.Once);
            _providerResponseEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            mockEntities.Should().AllSatisfy(e => e.ProviderResponseId = response.ProviderResponseId);
            _providerResponseEmployerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
