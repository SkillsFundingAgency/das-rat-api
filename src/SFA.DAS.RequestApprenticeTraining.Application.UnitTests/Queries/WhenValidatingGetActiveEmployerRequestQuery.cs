using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    [TestFixture]
    public class WhenValidatingGetActiveEmployerRequestQuery
    {
        private GetActiveEmployerRequestQueryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetActiveEmployerRequestQueryValidator();
        }

        [Test]
        public void And_StandardReferenceAndAccountId_AreProvided_Then_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetActiveEmployerRequestQuery
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
        public void And_StandardReferenceOrAccountId_IsMissing_Then_ShouldHaveValidationError()
        {
            // Arrange
            var query1 = new GetActiveEmployerRequestQuery
            {
                AccountId = 123
            };

            var query2 = new GetActiveEmployerRequestQuery
            {
                StandardReference = "ABC123"
            };

            // Act & Assert
            var result1 = _validator.TestValidate(query1);
            var result2 = _validator.TestValidate(query2);

            result1.ShouldHaveValidationErrorFor(x => new { x.StandardReference, x.AccountId })
                .WithErrorMessage("Both StandardReference and AccountId must be provided.");

            result2.ShouldHaveValidationErrorFor(x => new { x.StandardReference, x.AccountId })
                .WithErrorMessage("Both StandardReference and AccountId must be provided.");
        }
    }
}
