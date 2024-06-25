using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromAggregatedEmployerRequestEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(AggregatedEmployerRequest source)
        {
            var result = (Domain.Models.AggregatedEmployerRequest)source;

            result.CourseReference.Should().Be(source.CourseReference);
            result.CourseTitle.Should().Be(source.CourseTitle);
            result.Level.Should().Be(source.Level);
            result.Sector.Should().Be(source.Sector);
        }
    }
}
