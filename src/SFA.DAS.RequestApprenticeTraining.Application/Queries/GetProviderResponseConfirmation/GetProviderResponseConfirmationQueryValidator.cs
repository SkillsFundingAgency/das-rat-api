using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{ 
    public class GetProviderResponseConfirmationQueryValidator : AbstractValidator<GetProviderResponseConfirmationQuery>
    {
        public GetProviderResponseConfirmationQueryValidator()
        {
            RuleFor(x => x.ProviderResponseId)
                .NotEmpty()
                .WithMessage("ProviderResponseid must not be an empty GUID.");
        }
    }
}
