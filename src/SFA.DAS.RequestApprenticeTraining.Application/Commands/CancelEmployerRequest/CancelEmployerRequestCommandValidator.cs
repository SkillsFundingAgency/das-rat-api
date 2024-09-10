using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest
{
    public class CancelEmployerRequestCommandValidator : AbstractValidator<CancelEmployerRequestCommand>
    {
        public CancelEmployerRequestCommandValidator()
        {
            RuleFor(x => x.EmployerRequestId)
                .NotEmpty().WithMessage("Employer Request Id must not be empty.");

            RuleFor(x => x.CancelledBy)
                .NotEmpty().WithMessage("Cancelled By must not be empty.");
        }
    }
}
