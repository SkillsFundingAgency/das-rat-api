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

            result.StandardReference.Should().Be(source.StandardReference);
            result.StandardTitle.Should().Be(source.StandardTitle);
            result.StandardLevel.Should().Be(source.StandardLevel);
            result.StandardSector.Should().Be(source.StandardSector);
        }
    }
}
