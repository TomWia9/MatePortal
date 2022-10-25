using FluentValidation;

namespace Application.Brands.Commands.CreateBrand;

/// <summary>
///     CreateBrandCommand validator
/// </summary>
public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    /// <summary>
    ///     Initializes CreateBrandCommandValidator
    /// </summary>
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(60)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .NotEmpty();
    }
}