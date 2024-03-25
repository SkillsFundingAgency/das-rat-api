using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsQueryValidator : AbstractValidator<GetEmployerRequestsQuery>
    {
        public GetEmployerRequestsQueryValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
