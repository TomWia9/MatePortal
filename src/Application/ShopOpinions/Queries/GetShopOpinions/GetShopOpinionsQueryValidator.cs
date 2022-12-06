using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using FluentValidation;

namespace Application.ShopOpinions.Queries.GetShopOpinions;

/// <summary>
///     GetShopOpinionsQuery validator
/// </summary>
public class GetShopOpinionsQueryValidator : AbstractValidator<ShopOpinionsQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(ShopOpinion.Rate).ToLower(),
        nameof(ShopOpinion.Comment).ToLower(),
        nameof(ShopOpinion.Created).ToLower()
    };

    /// <summary>
    ///     Initializes GetShopOpinionsQueryValidator
    /// </summary>
    public GetShopOpinionsQueryValidator()
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