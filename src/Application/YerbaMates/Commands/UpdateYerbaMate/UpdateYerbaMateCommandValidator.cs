using FluentValidation;

namespace Application.YerbaMates.Commands.UpdateYerbaMate;

/// <summary>
///     UpdateYerbaMateCommand validator
/// </summary>
public class UpdateYerbaMateCommandValidator : AbstractValidator<UpdateYerbaMateCommand>
{
    /// <summary>
    ///     Initializes UpdateYerbaMateCommandValidator
    /// </summary>
    public UpdateYerbaMateCommandValidator()
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