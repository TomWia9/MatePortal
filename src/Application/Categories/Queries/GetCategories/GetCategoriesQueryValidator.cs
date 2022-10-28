using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using FluentValidation;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
///     GetCategoriesQuery validator
/// </summary>
public class GetCategoriesQueryValidator : AbstractValidator<CategoriesQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(Category.Name).ToLower(),
        nameof(Category.Description).ToLower()
    };
    
    /// <summary>
    ///     Initializes GetCategoriesQueryValidator
    /// </summary>
    public GetCategoriesQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");
    }
}