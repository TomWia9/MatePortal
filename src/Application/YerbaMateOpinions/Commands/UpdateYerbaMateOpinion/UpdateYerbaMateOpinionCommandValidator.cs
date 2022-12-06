using FluentValidation;

namespace Application.YerbaMateOpinions.Commands.UpdateYerbaMateOpinion;

/// <summary>
///     UpdateYerbaMateOpinionCommand validator
/// </summary>
public class UpdateYerbaMateOpinionCommandValidator: AbstractValidator<UpdateYerbaMateOpinionCommand>
{
    /// <summary>
    ///     Initializes UpdateYerbaMateOpinionCommandValidator
    /// </summary>
    public UpdateYerbaMateOpinionCommandValidator()
    {
        RuleFor(x => x.Rate)
            .NotEmpty()
            .InclusiveBetween(1, 10);

        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(500);
    }
}