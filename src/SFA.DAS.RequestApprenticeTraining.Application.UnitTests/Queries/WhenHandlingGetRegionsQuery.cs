using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetRegions;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries.GetRegions
{
    public class WhenHandlingGetRegionsQuery
    {
        private Mock<IRegionEntityContext> _regionEntityContextMock;
        private GetRegionsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _regionEntityContextMock = new Mock<IRegionEntityContext>();
            _handler = new GetRegionsQueryHandler(_regionEntityContextMock.Object);
        }

        [Test]
        public async Task Then_Returns_All_Regions_When_Found()
        {
            // Arrange
            var query = new GetRegionsQuery();
            var regions = new List<Region>
            {
                new Region { Id = 1, SubregionName = "London" },
                new Region { Id = 2, SubregionName = "Manchester" }
            };
            _regionEntityContextMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(regions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Regions.Should().BeEquivalentTo(regions, options => options
                .Excluding(r => r.EmployerRequestRegions));
        }

        [Test]
        public async Task Then_Returns_Empty_List_When_No_Regions_Found()
        {
            // Arrange
            var query = new GetRegionsQuery();
            var regions = new List<Region>();
            _regionEntityContextMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(regions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Regions.Should().BeEmpty();
        }
    }
}
