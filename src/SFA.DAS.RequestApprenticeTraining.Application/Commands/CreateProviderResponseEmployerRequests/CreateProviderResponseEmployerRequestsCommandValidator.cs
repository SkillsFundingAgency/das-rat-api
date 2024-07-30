using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests
{
    public class CreateProviderResponseEmployerRequestsCommandValidator : AbstractValidator<CreateProviderResponseEmployerRequestsCommand>
    {
        public CreateProviderResponseEmployerRequestsCommandValidator()
        {
            RuleFor(x => x.EmployerRequestIds)
                .NotEmpty().WithMessage("Employer Requests must not be empty.");

            RuleFor(x => x.Ukprn)
                .GreaterThan(0).WithMessage("Ukprn must be greater than zero.");
        }
    }
}
