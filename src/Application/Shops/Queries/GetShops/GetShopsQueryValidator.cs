using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using FluentValidation;

namespace Application.Shops.Queries.GetShops;

/// <summary>
///     GetShopsQuery validator
/// </summary>
public class GetShopsQueryValidator : AbstractValidator<ShopsQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(Shop.Name).ToLower(),
        nameof(Shop.Description).ToLower(),
        nameof(Shop.Opinions).ToLower()
    };
    
    /// <summary>
    ///     GetShopsQueryValidator
    /// </summary>
    public GetShopsQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");
    }
}