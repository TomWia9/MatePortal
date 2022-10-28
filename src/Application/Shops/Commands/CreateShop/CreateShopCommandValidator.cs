using System;
using FluentValidation;

namespace Application.Shops.Commands.CreateShop;

/// <summary>
///     CreateShopCommand validator
/// </summary>
public class CreateShopCommandValidator : AbstractValidator<CreateShopCommand>
{
    /// <summary>
    ///     Initializes CreateShopCommandValidator
    /// </summary>
    public CreateShopCommandValidator()
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