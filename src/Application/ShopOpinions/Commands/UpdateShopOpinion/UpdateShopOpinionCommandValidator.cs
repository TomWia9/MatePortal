using FluentValidation;

namespace Application.ShopOpinions.Commands.UpdateShopOpinion;

/// <summary>
///     UpdateShopOpinionCommand validator
/// </summary>
public class UpdateShopOpinionCommandValidator : AbstractValidator<UpdateShopOpinionCommand>
{
    /// <summary>
    ///     Initializes UpdateShopOpinionCommandValidator
    /// </summary>
    public UpdateShopOpinionCommandValidator()
    {
        RuleFor(x => x.Rate)
            .NotEmpty()
            .InclusiveBetween(1, 10);

        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(500);
    }
}