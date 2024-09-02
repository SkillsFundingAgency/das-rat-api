using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromSelectEmployerRequestEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(SelectEmployerRequest source)
        {
            var result = (Domain.Models.SelectEmployerRequest)source;

            result.EmployerRequestId.Should().Be(source.EmployerRequestId);
            result.StandardReference.Should().Be(source.StandardReference);
            result.StandardTitle.Should().Be(source.StandardTitle);
            result.StandardLevel.Should().Be(source.StandardLevel);
            result.SingleLocation.Should().Be(source.SingleLocation);
            result.DateOfRequest.Should().Be(source.DateOfRequest);
            result.NumberOfApprentices.Should().Be(source.NumberOfApprentices);
            result.DayRelease.Should().Be(source.DayRelease);
            result.BlockRelease.Should().Be(source.BlockRelease);
            result.AtApprenticesWorkplace.Should().Be(source.AtApprenticesWorkplace);
            result.IsNew.Should().Be(source.IsNew);
            result.IsContacted.Should().Be(source.IsContacted);
            result.DateContacted.Should().Be(source.DateContacted);
            result.Locations.Should().BeEquivalentTo(source.Locations);
        }
    }
}
