using FluentValidation;

namespace Application.ShopOpinions.Commands.CreateShopOpinion;

/// <summary>
///     CreateShopOpinionCommand validator
/// </summary>
public class CreateShopOpinionCommandValidator : AbstractValidator<CreateShopOpinionCommand>
{
    /// <summary>
    ///     Initializes CreateShopOpinionCommandValidator
    /// </summary>
    public CreateShopOpinionCommandValidator()
    {
        RuleFor(x => x.Rate)
            .NotEmpty()
            .InclusiveBetween(1, 10);

        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(500);
    }
}