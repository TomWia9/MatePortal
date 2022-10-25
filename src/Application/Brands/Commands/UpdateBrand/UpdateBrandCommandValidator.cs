using FluentValidation;

namespace Application.Brands.Commands.UpdateBrand;

/// <summary>
///     UpdateBrandCommand validator
/// </summary>
public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    /// <summary>
    ///     Initializes CreateBrandCommandValidator
    /// </summary>
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(60)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .NotEmpty();
    }
}