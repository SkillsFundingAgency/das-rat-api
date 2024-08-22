using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommandValidator : AbstractValidator<SubmitProviderResponseCommand>
    {
        public SubmitProviderResponseCommandValidator()
        {
            RuleFor(x => x.EmployerRequestIds)
                .NotEmpty().WithMessage("Employer Requests must not be empty.");

            RuleFor(x => x.Ukprn)
                .GreaterThan(0).WithMessage("Ukprn must be greater than zero.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must not be empty.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone must not be empty.");

            RuleFor(x => x.RespondedBy)
                .NotEmpty().WithMessage("Responded By must not be empty.");

        }
    }
}
