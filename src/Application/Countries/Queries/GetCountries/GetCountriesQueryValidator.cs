using System.Collections.Generic;
using System.Linq;
using Application.Common.QueryParameters;
using Domain.Entities;
using FluentValidation;

namespace Application.Countries.Queries.GetCountries;

/// <summary>
///     GetCountriesQuery validator
/// </summary>
public class GetCountriesQueryValidator : QueryValidator<CountriesQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(Country.Name).ToLower(),
        nameof(Country.Description).ToLower()
    };
    
    /// <summary>
    ///     Initializes GetCountriesQueryValidator
    /// </summary>
    public GetCountriesQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");
    }
}