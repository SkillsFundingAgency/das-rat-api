using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest
{
    public class GetActiveEmployerRequestQueryValidator : AbstractValidator<GetActiveEmployerRequestQuery>
    {
        public GetActiveEmployerRequestQueryValidator()
        {
            RuleFor(x => new { x.StandardReference, x.AccountId })
                .Must(x => !string.IsNullOrEmpty(x.StandardReference) && x.AccountId.HasValue)
                .WithMessage("Both StandardReference and AccountId must be provided.");
        }
    }
}
