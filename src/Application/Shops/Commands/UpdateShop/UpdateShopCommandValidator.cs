using System;
using FluentValidation;

namespace Application.Shops.Commands.UpdateShop;

/// <summary>
///     UpdateShopCommand validator
/// </summary>
public class UpdateShopCommandValidator : AbstractValidator<UpdateShopCommand>
{
    /// <summary>
    ///     Initializes UpdateShopCommandValidator
    /// </summary>
    public UpdateShopCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Url)
            .NotEmpty()
            .MaximumLength(200)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("The url field has an invalid format.");
    }
}