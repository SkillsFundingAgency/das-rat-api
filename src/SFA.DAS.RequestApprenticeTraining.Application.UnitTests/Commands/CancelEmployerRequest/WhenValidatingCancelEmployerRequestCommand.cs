using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands.CancelEmployerRequest
{
    [TestFixture]
    public class WhenValidatingCancelEmployerRequestCommand
    {
        private CancelEmployerRequestCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CancelEmployerRequestCommandValidator();
        }

        [Test]
        public void Then_Validation_Should_Fail_When_EmployerRequestId_Is_Empty()
        {
            // Arrange
            var command = new CancelEmployerRequestCommand
            {
                EmployerRequestId = Guid.Empty,
                CancelledBy = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(c => c.EmployerRequestId)
                  .WithErrorMessage("Employer Request Id must not be empty.");
        }

        [Test]
        public void Then_Validation_Should_Fail_When_CancelledBy_Is_Empty()
        {
            // Arrange
            var command = new CancelEmployerRequestCommand
            {
                EmployerRequestId = Guid.NewGuid(),
                CancelledBy = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(c => c.CancelledBy)
                  .WithErrorMessage("Cancelled By must not be empty.");
        }

        [Test]
        public void Then_Validation_Should_Pass_When_EmployerRequestId_And_CancelledBy_Are_Valid()
        {
            // Arrange
            var command = new CancelEmployerRequestCommand
            {
                EmployerRequestId = Guid.NewGuid(),
                CancelledBy = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
