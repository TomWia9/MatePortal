using FluentValidation;

namespace Application.Categories.Commands.CreateCategory;

/// <summary>
///     CreateCategoryCommand validator
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    /// <summary>
    ///     Initializes CreateCategoryCommandValidator
    /// </summary>
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().MaximumLength(1000);
    }
}