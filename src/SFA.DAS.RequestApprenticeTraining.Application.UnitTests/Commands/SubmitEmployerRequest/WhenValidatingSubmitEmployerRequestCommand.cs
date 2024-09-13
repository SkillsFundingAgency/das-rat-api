using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.SubmitEmployerRequest
{
    [TestFixture]
    public class WhenValidatingSubmitEmployerRequestCommand
    {
        private SubmitEmployerRequestCommandValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SubmitEmployerRequestCommandValidator();
        }

        [Test]
        [TestCase(RequestType.Shortlist, TestName = "Valid RequestType")]
        [TestCase(RequestType.Providers, TestName = "Valid RequestType")]
        [TestCase(RequestType.CourseDetail, TestName = "Valid RequestType")]
        public void Validate_RequestType_ShouldHaveNoValidationError(RequestType requestType)
        {
            var model = new SubmitEmployerRequestCommand { RequestType = requestType };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.RequestType);
        }

        [Test]
        public void Validate_RequestType_Invalid_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { RequestType = (RequestType)999 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RequestType);
        }

        [Test]
        public void Validate_AccountId_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new SubmitEmployerRequestCommand { AccountId = 1 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.AccountId);
        }

        [Test]
        public void Validate_AccountId_ZeroOrNegative_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { AccountId = 0 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AccountId);
        }

        [Test]
        public void Validate_StandardReference_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitEmployerRequestCommand { StandardReference = "ST0010" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.StandardReference);
        }

        [Test]
        public void Validate_StandardReference_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { StandardReference = "" };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardReference);
        }

        [Test]
        public void Validate_NumberOfApprentices_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new SubmitEmployerRequestCommand { NumberOfApprentices = 1 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.NumberOfApprentices);
        }

        [Test]
        public void Validate_NumberOfApprentices_ZeroOrNegative_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { NumberOfApprentices = 0 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NumberOfApprentices);
        }

        [Test]
        public void Validate_SingleLocation_WhenSameLocationIsYes_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { SameLocation = "Yes", SingleLocation = "" };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.SingleLocation);
        }

        [Test]
        public void Validate_SingleLocationLatitude_WhenSameLocationIsYes_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { SameLocation = "Yes", SingleLocationLatitude = 91 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.SingleLocationLatitude);
        }

        [Test]
        public void Validate_SingleLocationLongitude_WhenSameLocationIsYes_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { SameLocation = "Yes", SingleLocationLongitude = 181 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.SingleLocationLongitude);
        }

        [Test]
        public void Validate_MultipleLocations_WhenSameLocationIsNo_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { SameLocation = "No", MultipleLocations = new string[0] };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MultipleLocations);
        }

        [Test]
        public void Validate_AtLeastOneOf_AtApprenticesWorkplace_DayRelease_BlockRelease_ShouldHaveNoValidationError_WhenAtLeastOneIsTrue()
        {
            var model = new SubmitEmployerRequestCommand { AtApprenticesWorkplace = true, DayRelease = false, BlockRelease = false };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => new { x.AtApprenticesWorkplace, x.DayRelease, x.BlockRelease });

            model = new SubmitEmployerRequestCommand { AtApprenticesWorkplace = false, DayRelease = true, BlockRelease = false };
            result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => new { x.AtApprenticesWorkplace, x.DayRelease, x.BlockRelease });

            model = new SubmitEmployerRequestCommand { AtApprenticesWorkplace = false, DayRelease = false, BlockRelease = true };
            result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => new { x.AtApprenticesWorkplace, x.DayRelease, x.BlockRelease });
        }

        [Test]
        public void Validate_AtLeastOneOf_AtApprenticesWorkplace_DayRelease_BlockRelease_ShouldHaveValidationError_WhenAllAreFalse()
        {
            var model = new SubmitEmployerRequestCommand { AtApprenticesWorkplace = false, DayRelease = false, BlockRelease = false };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => new { x.AtApprenticesWorkplace, x.DayRelease, x.BlockRelease });
        }

        [Test]
        public void Validate_RequestedBy_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitEmployerRequestCommand { RequestedBy = Guid.NewGuid() };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.RequestedBy);
        }

        [Test]
        public void Validate_RequestedBy_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { RequestedBy = Guid.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RequestedBy);
        }

        [Test]
        public void Validate_ModifiedBy_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitEmployerRequestCommand { ModifiedBy = Guid.NewGuid() };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ModifiedBy);
        }

        [Test]
        public void Validate_ModifiedBy_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitEmployerRequestCommand { ModifiedBy = Guid.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ModifiedBy);
        }
    }
}