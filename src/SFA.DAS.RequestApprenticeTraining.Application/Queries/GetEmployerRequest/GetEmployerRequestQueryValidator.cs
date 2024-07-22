using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQueryValidator : AbstractValidator<GetEmployerRequestQuery>
    {
        public GetEmployerRequestQueryValidator()
        {
            RuleFor(x => x.EmployerRequestId)
                .Must(x => x.HasValue)
                .When(x => string.IsNullOrEmpty(x.StandardReference) || !x.AccountId.HasValue)
                .WithMessage("EmployerRequestId must be provided if StandardReference or AccountId is missing.");

            RuleFor(x => new { x.StandardReference, x.AccountId })
                .Must(x => !string.IsNullOrEmpty(x.StandardReference) && x.AccountId.HasValue)
                .When(x => !x.EmployerRequestId.HasValue)
                .WithMessage("Both StandardReference and AccountId must be provided if EmployerRequestId is missing.");
        }
    }
}
