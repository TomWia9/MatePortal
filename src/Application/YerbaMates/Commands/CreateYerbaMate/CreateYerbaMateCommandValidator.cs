using FluentValidation;

namespace Application.YerbaMates.Commands.CreateYerbaMate;

/// <summary>
///     CreateYerbaMateCommand validator
/// </summary>
public class CreateYerbaMateCommandValidator : AbstractValidator<CreateYerbaMateCommand>
{
    /// <summary>
    ///     Initializes CreateYerbaMateCommandValidator
    /// </summary>
    public CreateYerbaMateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.AveragePrice)
            .NotEmpty()
            .InclusiveBetween(0, 1000);
    }
}