using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromProviderAggregatedEmployerRequestToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(ProviderAggregatedEmployerRequest source)
        {
            var result = (Domain.Models.ProviderAggregatedEmployerRequest)source;

            result.StandardReference.Should().Be(source.StandardReference);
            result.StandardLevel.Should().Be(source.StandardLevel);
            result.StandardTitle.Should().Be(source.StandardTitle);
            result.StandardSector.Should().Be(source.StandardSector);
            result.NumberOfApprentices.Should().Be(source.NumberOfApprentices);
            result.NumberOfEmployers.Should().Be(source.NumberOfEmployers);
            result.IsNew.Should().Be(source.IsNew);
        }
    }
}
