using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries.GetClosestRegion
{
    public class WhenHandlingGetClosestRegionQuery
    {
        private Mock<IRegionEntityContext> _regionEntityContextMock;
        private GetClosestRegionQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _regionEntityContextMock = new Mock<IRegionEntityContext>();
            _handler = new GetClosestRegionQueryHandler(_regionEntityContextMock.Object);
        }

        [Test]
        public async Task Then_Returns_Closest_Region_When_Found()
        {
            // Arrange
            var query = new GetClosestRegionQuery { Latitude = 51.5074, Longitude = -0.1278 }; // Example coordinates
            var region = new Region { Id = 1, SubregionName = "London" };

            _regionEntityContextMock
                .Setup(x => x.FindClosestRegion(query.Latitude, query.Longitude))
                .ReturnsAsync(region);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Region.Should().BeEquivalentTo(region, options => options
                .Excluding(r => r.EmployerRequestRegions));
        }

        [Test]
        public async Task Then_Returns_Null_When_No_Region_Found()
        {
            // Arrange
            var query = new GetClosestRegionQuery { Latitude = 51.5074, Longitude = -0.1278 }; // Example coordinates
            _regionEntityContextMock
                .Setup(x => x.FindClosestRegion(query.Latitude, query.Longitude))
                .ReturnsAsync((Region)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Region.Should().BeNull();
        }
    }
}
