using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQueryValidator : AbstractValidator<GetEmployerRequestQuery>
    {
        public GetEmployerRequestQueryValidator()
        {
            RuleFor(x => x.EmployerRequestId).NotEmpty();
        }
    }
}
