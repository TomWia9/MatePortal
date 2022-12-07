using System.Collections.Generic;
using System.Linq;
using Application.Common.QueryParameters;
using Domain.Entities;
using FluentValidation;

namespace Application.YerbaMates.Queries.GetYerbaMates;

/// <summary>
///     GetYerbaMatesQuery validator
/// </summary>
public class GetYerbaMatesQueryValidator : QueryValidator<YerbaMatesQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(YerbaMate.Name).ToLower(),
        nameof(YerbaMate.AveragePrice).ToLower(),
        nameof(YerbaMate.YerbaMateOpinions).ToLower(),
        nameof(YerbaMate.Favourites).ToLower()
    };

    /// <summary>
    ///     GetYerbaMatesQueryValidator
    /// </summary>
    public GetYerbaMatesQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");
    }
}