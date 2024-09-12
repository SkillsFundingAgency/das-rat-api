using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommandValidator : AbstractValidator<AcknowledgeProviderResponsesCommand>
    {
        public AcknowledgeProviderResponsesCommandValidator()
        {
            RuleFor(x => x.EmployerRequestId)
                .NotEmpty().WithMessage("Employer Request Id must not be empty.");

            RuleFor(x => x.AcknowledgedBy)
                .NotEmpty().WithMessage("Acknowleged By must not be empty.");
        }
    }
}
