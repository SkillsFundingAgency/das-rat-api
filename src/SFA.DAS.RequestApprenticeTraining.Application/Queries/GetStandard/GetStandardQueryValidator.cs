using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQueryValidator : AbstractValidator<GetStandardQuery>
    {
        public GetStandardQueryValidator()
        {
            RuleFor(x => x.StandardReference)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("StandardReference must be provided.");
        }
    }
}
