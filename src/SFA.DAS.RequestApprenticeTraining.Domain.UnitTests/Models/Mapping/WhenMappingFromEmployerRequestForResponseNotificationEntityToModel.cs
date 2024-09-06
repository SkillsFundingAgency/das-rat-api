using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RequestApprenticeTraining.Domain.UnitTests.Models
{
    public class WhenMappingFromEmployerRequestForResponseNotificationEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void ThenTheFieldsAreCorrectlyMapped(EmployerRequestForResponseNotification source)
        {
            var result = (Domain.Models.EmployerRequestForResponseNotification)source;

            result.AccountId.Should().Be(source.AccountId);
            result.RequestedBy.Should().Be(source.RequestedBy);
            result.Standards.Should().BeEquivalentTo(source.Standard);
        }
    }
}
