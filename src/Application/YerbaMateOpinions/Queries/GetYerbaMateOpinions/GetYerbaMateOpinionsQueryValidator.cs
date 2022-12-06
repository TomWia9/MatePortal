using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using FluentValidation;

namespace Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;

/// <summary>
///     GetYerbaMateOpinionsQuery validator
/// </summary>
public class GetYerbaMateOpinionsQueryValidator: AbstractValidator<YerbaMateOpinionsQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(YerbaMateOpinion.Rate).ToLower(),
        nameof(YerbaMateOpinion.Comment).ToLower(),
        nameof(YerbaMateOpinion.Created).ToLower()
    };

    /// <summary>
    ///     Initializes GetYerbaMateOpinionsQueryValidator
    /// </summary>
    public GetYerbaMateOpinionsQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");

        RuleFor(x => x.MinRate)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(10);

        RuleFor(x => x.MaxRate)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(10);
    }
}