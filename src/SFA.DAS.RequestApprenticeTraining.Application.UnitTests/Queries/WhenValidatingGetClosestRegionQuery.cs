using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    [TestFixture]
    public class WhenValidatingGetClosestRegionQuery
    {
        private GetClosestRegionQueryValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new GetClosestRegionQueryValidator();
        }

        [Test]
        public void Validate_Latitude_InclusiveBetween_ShouldHaveNoValidationError()
        {
            var model = new GetClosestRegionQuery { Latitude = 45 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Latitude);
        }

        [Test]
        public void Validate_Latitude_NotInclusiveBetween_ShouldHaveValidationError()
        {
            var model = new GetClosestRegionQuery { Latitude = 100 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Latitude).WithErrorMessage("Latitude must be between -90 and 90.");
        }

        [Test]
        public void Validate_Longitude_InclusiveBetween_ShouldHaveNoValidationError()
        {
            var model = new GetClosestRegionQuery { Longitude = 45 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Longitude);
        }

        [Test]
        public void Validate_Longitude_NotInclusiveBetween_ShouldHaveValidationError()
        {
            var model = new GetClosestRegionQuery { Longitude = 200 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Longitude).WithErrorMessage("Longitude must be between -180 and 180.");
        }
    }
}
