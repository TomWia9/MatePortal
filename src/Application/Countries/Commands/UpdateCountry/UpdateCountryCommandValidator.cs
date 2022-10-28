using FluentValidation;

namespace Application.Countries.Commands.UpdateCountry;

/// <summary>
///     UpdateCountryCommand validator
/// </summary>
public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    /// <summary>
    ///     Initializes UpdateCountryCommandValidator
    /// </summary>
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().MaximumLength(1000);
    }
}