﻿using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using FluentValidation;

namespace Application.Brands.Queries.GetBrands;

/// <summary>
///     GetBrandsQuery validator
/// </summary>
public class GetBrandsQueryValidator : AbstractValidator<BrandsQueryParameters>
{
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        nameof(Brand.Name).ToLower(),
        nameof(Brand.Description).ToLower(),
        nameof(Brand.Country).ToLower()
    };
    
    /// <summary>
    ///     Initializes GetBrandsQueryValidator
    /// </summary>
    public GetBrandsQueryValidator()
    {
        RuleFor(x => x.Country).MaximumLength(50);

        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");
    }
}