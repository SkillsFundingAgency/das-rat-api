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
        public void And_EmployerRequestId_IsNotProvided_Then_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetEmployerRequestQuery();

            // Act & Assert
            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.EmployerRequestId)
                .WithErrorMessage("EmployerRequestId must be provided.");
        }
    }
}
