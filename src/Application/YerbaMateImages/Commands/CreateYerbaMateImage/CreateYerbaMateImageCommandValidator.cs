using System;
using FluentValidation;

namespace Application.YerbaMateImages.Commands.CreateYerbaMateImage;

/// <summary>
///     CreateYerbaMateImageCommand validator
/// </summary>
public class CreateYerbaMateImageCommandValidator : AbstractValidator<CreateYerbaMateImageCommand>
{
    /// <summary>
    ///     Initializes CreateYerbaMateImageCommandValidator
    /// </summary>
    public CreateYerbaMateImageCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .MaximumLength(200)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("The url field has an invalid format.");
    }
}