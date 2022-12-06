using FluentValidation;

namespace Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;

/// <summary>
///     CreateYerbaMateOpinionCommand validator
/// </summary>
public class CreateYerbaMateOpinionCommandValidator : AbstractValidator<CreateYerbaMateOpinionCommand>
{
    /// <summary>
    ///     Initializes CreateYerbaMateOpinionCommandValidator
    /// </summary>
    public CreateYerbaMateOpinionCommandValidator()
    {
        RuleFor(x => x.Rate)
            .NotEmpty()
            .InclusiveBetween(1, 10);

        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(500);
    }
}