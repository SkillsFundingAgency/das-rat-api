using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands.AcknowledgeProviderResponses
{
    [TestFixture]
    public class WhenValidatingAcknowledgeProviderResponsesCommand
    {
        private AcknowledgeProviderResponsesCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new AcknowledgeProviderResponsesCommandValidator();
        }

        [Test]
        public void Then_Validation_Should_Fail_When_EmployerRequestId_Is_Empty()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand
            {
                EmployerRequestId = Guid.Empty,
                AcknowledgedBy = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(c => c.EmployerRequestId)
                  .WithErrorMessage("Employer Request Id must not be empty.");
        }

        [Test]
        public void Then_Validation_Should_Fail_When_AcknowledgedBy_Is_Empty()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand
            {
                EmployerRequestId = Guid.NewGuid(),
                AcknowledgedBy = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(c => c.AcknowledgedBy)
                  .WithErrorMessage("Acknowleged By must not be empty.");
        }

        [Test]
        public void Then_Validation_Should_Pass_When_EmployerRequestId_And_AcknowledgedBy_Are_Valid()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand
            {
                EmployerRequestId = Guid.NewGuid(),
                AcknowledgedBy = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
