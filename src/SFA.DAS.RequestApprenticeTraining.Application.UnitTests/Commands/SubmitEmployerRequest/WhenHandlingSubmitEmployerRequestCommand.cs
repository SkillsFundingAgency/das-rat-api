using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.SubmitEmployerRequest
{
    [TestFixture]
    public class WhenHandlingSubmitEmployerRequestCommand
    {
        private Mock<IEmployerRequestEntityContext> _employerRequestEntityContextMock;
        private Mock<IEmployerRequestRegionEntityContext> _employerRequestRegionEntityContextMock;
        private Mock<IRegionEntityContext> _regionEntityContextMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private SubmitEmployerRequestCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            var dateTimeNow = new DateTime(2025, 05, 01, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.SetupGet(s => s.Now).Returns(dateTimeNow);

            _employerRequestEntityContextMock = new Mock<IEmployerRequestEntityContext>();
            _employerRequestRegionEntityContextMock = new Mock<IEmployerRequestRegionEntityContext>();
            _regionEntityContextMock = new Mock<IRegionEntityContext>();

            _sut = new SubmitEmployerRequestCommandHandler(
                _employerRequestEntityContextMock.Object,
                _employerRequestRegionEntityContextMock.Object,
                _regionEntityContextMock.Object,
                _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateEmployerRequestWithMatchingClosestRegion()
        {
            // Arrange
            var command = new SubmitEmployerRequestCommand
            {
                OriginalLocation = string.Empty,
                RequestType = Domain.Models.Enums.RequestType.Shortlist,
                AccountId = 123,
                StandardReference = "STD123",
                NumberOfApprentices = 5,
                SingleLocation = "Telford",
                AtApprenticesWorkplace = false,
                DayRelease = true,
                BlockRelease = false,
                RequestedBy = Guid.NewGuid(),
                SingleLocationLatitude = 51.509865,
                SingleLocationLongitude = -0.118092,
                ModifiedBy = Guid.NewGuid(),
            };

            var region = new Region { Id = 2 };
            _regionEntityContextMock.Setup(x => x.FindClosestRegion(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(region);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.Add(It.IsAny<EmployerRequest>()), Times.Once);
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _regionEntityContextMock.Verify(x => x.FindClosestRegion(command.SingleLocationLatitude, command.SingleLocationLongitude), Times.Once);

            _employerRequestRegionEntityContextMock.Verify(x => x.Add(It.Is<EmployerRequestRegion>(x => x.RegionId == region.Id)), Times.Once);
            _employerRequestRegionEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
