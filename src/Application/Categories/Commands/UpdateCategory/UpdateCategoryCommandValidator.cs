using FluentValidation;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
///     UpdateCategoryCommand validator
/// </summary>
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    /// <summary>
    ///     Initializes UpdateCategoryCommandValidator
    /// </summary>
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().MaximumLength(1000);
    }
}