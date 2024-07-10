using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    [TestFixture]
    public class WhenValidatingGetEmployerRequestQuery
    {
        private GetEmployerRequestQueryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetEmployerRequestQueryValidator();
        }

        [Test]
        public void And_EmployerRequestId_IsProvided_Then_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetEmployerRequestQuery
            {
                EmployerRequestId = Guid.NewGuid()
            };

            // Act & Assert
            var result = _validator.TestValidate(query);

            result.ShouldNotHaveValidationErrorFor(x => x.EmployerRequestId);
        }

        [Test]
        public void And_StandardReferenceAndAccountId_AreProvided_Then_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetEmployerRequestQuery
            {
                StandardReference = "ABC123",
                AccountId = 123
            };

            // Act & Assert
            var result = _validator.TestValidate(query);

            result.ShouldNotHaveValidationErrorFor(x => x.StandardReference);
            result.ShouldNotHaveValidationErrorFor(x => x.AccountId);
        }

        [Test]
        public void And_EmployerRequestId_IsNotProvided_And_StandardReferenceAndAccountId_AreMissing_Then_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetEmployerRequestQuery();

            // Act & Assert
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.EmployerRequestId)
                .WithErrorMessage("EmployerRequestId must be provided if StandardReference or AccountId is missing.");
        }

        [Test]
        public void And_EmployerRequestId_IsNotProvided_And_StandardReferenceOrAccountId_IsMissing_Then_ShouldHaveValidationError()
        {
            // Arrange
            var query1 = new GetEmployerRequestQuery
            {
                AccountId = 123
            };

            var query2 = new GetEmployerRequestQuery
            {
                StandardReference = "ABC123"
            };

            // Act & Assert
            var result1 = _validator.TestValidate(query1);
            var result2 = _validator.TestValidate(query2);

            result1.ShouldHaveValidationErrorFor(x => new { x.StandardReference, x.AccountId })
                .WithErrorMessage("Both StandardReference and AccountId must be provided if EmployerRequestId is missing.");

            result2.ShouldHaveValidationErrorFor(x => new { x.StandardReference, x.AccountId })
                .WithErrorMessage("Both StandardReference and AccountId must be provided if EmployerRequestId is missing.");
        }
    }
}
