using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands
{
    [TestFixture]
    public class WhenValidatingUpdateProviderResponseStatusCommand
    {
        private UpdateProviderResponseStatusCommandValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new UpdateProviderResponseStatusCommandValidator();
        }

        [Test]
        public void Validate_EmployerRequestIds_ShouldHaveNoValidationError()
        {
            var model = new UpdateProviderResponseStatusCommand { EmployerRequestIds = new List<Guid> { new()} };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_EmployerRequestIds_Empty_ShouldHaveValidationError()
        {
            var model = new UpdateProviderResponseStatusCommand { EmployerRequestIds = new List<Guid>() };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_Ukprn_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new UpdateProviderResponseStatusCommand { Ukprn = 123456789 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Ukprn);
        }

        [Test]
        public void Validate_Ukprn_ZeroOrnegative_ShouldHaveNoValidationError()
        {
            var model = new UpdateProviderResponseStatusCommand { Ukprn = 0 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Ukprn);
        }

        [Test]
        public void Validate_ResponseStatus_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new UpdateProviderResponseStatusCommand { ProviderResponseStatus = 1 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ProviderResponseStatus);
        }

        [Test]
        public void Validate_ResponseStatus_ZeroOrnegative_ShouldHaveValidationError()
        {
            var model = new UpdateProviderResponseStatusCommand { ProviderResponseStatus = 0 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProviderResponseStatus);
        }
    }
}