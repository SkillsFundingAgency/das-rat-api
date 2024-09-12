using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromEmployerRequestEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(EmployerRequest source)
        {
            // Arrange
            source.ProviderResponseEmployerRequests.ForEach(p => { p.ProviderResponse.ProviderResponseEmployerRequests = new List<ProviderResponseEmployerRequest>() { p }; });
            
            // Act
            var result = (Domain.Models.EmployerRequest)source;

            // Act
            result.Id.Should().Be(source.Id);
            result.AccountId.Should().Be(source.AccountId);
            result.RequestType.Should().Be(source.RequestType);
            result.StandardReference.Should().Be(source.StandardReference);
            result.NumberOfApprentices.Should().Be(source.NumberOfApprentices);
            result.SameLocation.Should().Be(source.SameLocation);
            result.SingleLocation.Should().Be(source.SingleLocation);
            result.AtApprenticesWorkplace.Should().Be(source.AtApprenticesWorkplace);
            result.DayRelease.Should().Be(source.DayRelease);
            result.BlockRelease.Should().Be(source.BlockRelease);
            result.RequestStatus.Should().Be(source.RequestStatus);
            result.RequestedBy.Should().Be(source.RequestedBy);
            result.RequestedAt.Should().Be(source.RequestedAt);
            result.ModifiedBy.Should().Be(source.ModifiedBy);
        }
    }
}
