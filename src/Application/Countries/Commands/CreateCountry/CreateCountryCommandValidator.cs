using FluentValidation;

namespace Application.Countries.Commands.CreateCountry;

/// <summary>
///     CreateCountryCommand validator
/// </summary>
public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
    /// <summary>
    ///     Initializes CreateCountryCommandValidator
    /// </summary>
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().MaximumLength(1000);
    }
}