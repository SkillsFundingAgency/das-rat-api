using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromEmployerRequestEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(EmployerRequest source)
        {
            var result = (Domain.Models.EmployerRequest)source;

            result.Id.Should().Be(source.Id);
            result.RequestTypeId.Should().Be(source.RequestTypeId);
        }
    }
}
