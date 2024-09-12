using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQueryValidator : AbstractValidator<GetEmployerRequestQuery>
    {
        public GetEmployerRequestQueryValidator()
        {
            RuleFor(x => x.EmployerRequestId)
                .Must(x => x.HasValue)
                .WithMessage("EmployerRequestId must be provided.");
        }
    }
}
