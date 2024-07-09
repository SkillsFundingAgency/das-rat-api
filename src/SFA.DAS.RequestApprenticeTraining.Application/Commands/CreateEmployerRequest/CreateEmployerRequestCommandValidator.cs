using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommandValidator : AbstractValidator<CreateEmployerRequestCommand>
    {
        public CreateEmployerRequestCommandValidator()
        {
            RuleFor(x => x.RequestType)
                .IsInEnum().WithMessage("Request Type must be a valid enum value.");

            RuleFor(x => x.AccountId)
                .GreaterThan(0).WithMessage("Account Id must be greater than zero.");

            RuleFor(x => x.StandardReference)
                .NotEmpty().WithMessage("Standard Reference must not be empty.");

            RuleFor(x => x.NumberOfApprentices)
                .GreaterThan(0).WithMessage("Number Of Apprentices must be greater than zero.");

            When(x => string.IsNullOrEmpty(x.SameLocation) || x.SameLocation == "Yes", () =>
            {
                RuleFor(x => x.SingleLocation)
                    .NotEmpty().WithMessage("Single Location must not be empty.");

                RuleFor(x => x.SingleLocationLatitude)
                    .NotEmpty().WithMessage("Single Location Latitude must not be empty.");
                RuleFor(x => x.SingleLocationLatitude)
                    .InclusiveBetween(-90, 90).WithMessage("Single Location Latitude must be between -90 and 90.");

                RuleFor(x => x.SingleLocationLongitude)
                    .NotEmpty().WithMessage("Single Location Longitude must not be empty.");
                RuleFor(x => x.SingleLocationLongitude)
                    .InclusiveBetween(-180, 180).WithMessage("Single Location Longitude must be between -180 and 180.");
            }).Otherwise(() =>
            {
                RuleFor(x => x.MultipleLocations)
                    .NotEmpty().WithMessage("Multiple Locations must not be empty.");
            });

            RuleFor(x => new { x.AtApprenticesWorkplace, x.DayRelease, x.BlockRelease })
                .Must(x => x.AtApprenticesWorkplace || x.DayRelease || x.BlockRelease )
                .WithMessage("At least one of AtApprenticesWorkplace, DayRelease, or BlockRelease must be true.");

            RuleFor(x => x.RequestedBy)
                .NotEmpty().WithMessage("RequestedBy must not be empty.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("ModifiedBy must not be empty.");
        }
    }
}
