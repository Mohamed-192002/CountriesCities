using FluentValidation;
using CountriesCities.Application.DTOs;

namespace CountriesCities.Application.Validators;

public class CreateCityRequestValidator : AbstractValidator<CreateCityRequest>
{
    public CreateCityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("CountryId must be greater than zero.");
    }
}
