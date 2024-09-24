using FluentValidation;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.PostStandard
{
    public class PostStandardCommandValidator : AbstractValidator<PostStandardCommand>
    {
        public PostStandardCommandValidator()
        {
            RuleFor(x => x.StandardReference)
                .NotEmpty().WithMessage("Standard Reference must not be empty.");

            RuleFor(x => x.StandardTitle)
                .NotEmpty().WithMessage("Standard Title must not be empty.");

            RuleFor(x => x.StandardSector)
                .NotEmpty().WithMessage("Standard Sector must not be empty.");

            RuleFor(x => x.StandardLevel)
                .GreaterThan(0).WithMessage("Standard Level must be greater than zero.");
        }
    }
}
