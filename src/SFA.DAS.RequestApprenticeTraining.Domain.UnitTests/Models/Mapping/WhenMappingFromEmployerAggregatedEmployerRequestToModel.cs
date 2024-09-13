using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromEmployerAggregatedEmployerRequestToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(EmployerAggregatedEmployerRequest source)
        {
            var result = (Domain.Models.EmployerAggregatedEmployerRequest)source;

            result.StandardReference.Should().Be(source.StandardReference);
            result.StandardLevel.Should().Be(source.StandardLevel);
            result.StandardTitle.Should().Be(source.StandardTitle);
            result.RequestStatus.Should().Be(source.RequestStatus);
            result.RequestedAt.Should().Be(source.RequestedAt);
            result.NumberOfResponses.Should().Be(source.NumberOfResponses);
            result.NewNumberOfResponses.Should().Be(source.NewNumberOfResponses);
        }
    }
}
