using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion
{
    public class GetClosestRegionQueryValidator : AbstractValidator<GetClosestRegionQuery>
    {
        public GetClosestRegionQueryValidator()
        {
            RuleFor(x => x.Latitude)
                .NotEmpty().WithMessage("Latitude must not be empty.");
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .NotEmpty().WithMessage("Longitude must not be empty.");
            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}
