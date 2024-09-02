using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    [TestFixture]
    public class WhenValidatingGetProviderResponseConfirmation
    {
        private GetProviderResponseConfirmationQueryValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new GetProviderResponseConfirmationQueryValidator();
        }

        [Test]
        public void Validate_ValidGuid_ShouldHaveNoValidationError()
        {
            var model = new GetProviderResponseConfirmationQuery(Guid.NewGuid());
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ProviderResponseId);
        }

        [Test]
        public void Validate_EmptyGuid_ShouldHaveValidationError()
        {
            var model = new GetProviderResponseConfirmationQuery(new Guid());
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProviderResponseId);
        }
    }
}
