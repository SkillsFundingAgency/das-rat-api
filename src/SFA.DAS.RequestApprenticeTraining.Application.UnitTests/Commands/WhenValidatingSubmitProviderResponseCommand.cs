using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using System;
using System.Collections.Generic;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands
{
    [TestFixture]
    public class WhenValidatingSubmitProviderResponseCommand
    {
        private SubmitProviderResponseCommandValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SubmitProviderResponseCommandValidator();
        }

        [Test]
        public void Validate_Email_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitProviderResponseCommand { Email = "valid@email.com" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public void Validate_Email_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitProviderResponseCommand { Email = string.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public void Validate_Phone_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitProviderResponseCommand { Phone = "123456789" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Test]
        public void Validate_Phone_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitProviderResponseCommand { Phone = string.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [Test]
        public void Validate_Website_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitProviderResponseCommand { Website = "www.website.com" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Website);
        }

        [Test]
        public void Validate_Website_Empty_ShouldHaveNoValidationError()
        {
            var model = new SubmitProviderResponseCommand { Website = string.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Website);
        }

        [Test]
        public void Validate_EmployerRequests_GreaterThanZero_ShouldHaveNoValidationError()
        {
            var model = new SubmitProviderResponseCommand { EmployerRequestIds = new List<Guid>{ Guid.NewGuid(), Guid.NewGuid()} };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_EmployerRequests_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitProviderResponseCommand { EmployerRequestIds = new List<Guid>() };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_EmployerRequests_Null_ShouldHaveValidationError()
        {
            var model = new SubmitProviderResponseCommand { EmployerRequestIds = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.EmployerRequestIds);
        }

        [Test]
        public void Validate_ContactName_NotEmpty_ShouldHaveNoValidationError()
        {
            var model = new SubmitProviderResponseCommand { ContactName = "Firstname Surname" };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ContactName);
        }

        [Test]
        public void Validate_ContactName_Empty_ShouldHaveValidationError()
        {
            var model = new SubmitProviderResponseCommand { ContactName = string.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ContactName);
        }
    }
}