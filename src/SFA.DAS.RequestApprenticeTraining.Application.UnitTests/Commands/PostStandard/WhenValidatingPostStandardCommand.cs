using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.PostStandard;
using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.PostStandard
{
    [TestFixture]
    public class WhenValidatingPostStandardCommand
    {
        private PostStandardCommandValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new PostStandardCommandValidator();
        }

        [Test]
        public void Validate_StandardLevel_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new PostStandardCommand { StandardLevel = 1 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.StandardLevel);
        }

        [Test]
        public void Validate_StandardLevel_ZeroOrNegative_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardLevel = 0 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardLevel);
        }

        [Test]
        public void Validate_StandardReference_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new PostStandardCommand { StandardReference = "ST0010" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.StandardReference);
        }

        [Test]
        public void Validate_StandardReference_Empty_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardReference = "" };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardReference);
        }

        [Test]
        public void Validate_StandardReference_Null_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardReference = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardReference);
        }

        [Test]
        public void Validate_StandardTitle_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new PostStandardCommand { StandardTitle = "Engineering" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.StandardTitle);
        }

        [Test]
        public void Validate_StandardTitle_Empty_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardTitle = "" };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardTitle);
        }

        [Test]
        public void Validate_StandardTitle_Null_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardTitle = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardTitle);
        }

        [Test]
        public void Validate_StandardSector_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new PostStandardCommand { StandardSector = "Engineering" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.StandardSector);
        }

        [Test]
        public void Validate_StandardSector_Empty_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardSector = "" };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardSector);
        }

        [Test]
        public void Validate_StandardSector_Null_ShouldHaveValidationError()
        {
            var model = new PostStandardCommand { StandardSector = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StandardSector);
        }


    }
}