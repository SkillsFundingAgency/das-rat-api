using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.CreateProviderResponseEmployerRequests
{
    [TestFixture]
    public class WhenValidatingCreateProviderResponseEmployerRequestsCommand
    {
        private CreateProviderResponseEmployerRequestsCommandValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CreateProviderResponseEmployerRequestsCommandValidator();
        }

        [Test]
        public void Validate_EmployerRequestIds_ShouldHaveNoValidationError()
        {
            var model = new CreateProviderResponseEmployerRequestsCommand { EmployerRequestIds = new List<Guid> { new() } };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_EmployerRequestIds_Empty_ShouldHaveValidationError()
        {
            var model = new CreateProviderResponseEmployerRequestsCommand { EmployerRequestIds = new List<Guid>() };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_Ukprn_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new CreateProviderResponseEmployerRequestsCommand { Ukprn = 123456789 };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Ukprn);
        }

        [Test]
        public void Validate_Ukprn_ZeroOrnegative_ShouldHaveNoValidationError()
        {
            var model = new CreateProviderResponseEmployerRequestsCommand { Ukprn = 0 };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Ukprn);
        }
    }
}