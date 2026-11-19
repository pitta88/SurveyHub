using FluentValidation;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.Application.Institutions;

public sealed class CreateInstitutionRequestValidator : AbstractValidator<CreateInstitutionRequest>
{
    public CreateInstitutionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Institution.NameMaxLength);

        RuleFor(x => x.State)
            .NotEmpty()
            .Matches("^[A-Z]{2}$")
            .WithMessage("Should be capital 2 letters.");
    }
}