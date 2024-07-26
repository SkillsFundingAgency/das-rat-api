using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus
{
    public class UpdateProviderResponseStatusCommandValidator : AbstractValidator<UpdateProviderResponseStatusCommand>
    {
        public UpdateProviderResponseStatusCommandValidator()
        {
            RuleFor(x => x.EmployerRequestIds)
                .NotEmpty().WithMessage("Employer Requests must not be empty.");

            RuleFor(x => x.Ukprn)
                .GreaterThan(0).WithMessage("Account Id must be greater than zero.");

            RuleFor(x => x.ProviderResponseStatus)
                .GreaterThan(0).WithMessage("Response Status must be greater than zero.");
        }
    }
}
