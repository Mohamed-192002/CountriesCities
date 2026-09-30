using FluentValidation;
using CountriesCities.Application.DTOs;

namespace CountriesCities.Application.Validators;

public class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("CountryId must be greater than zero.");
    }
}
