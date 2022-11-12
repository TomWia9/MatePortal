﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace Application.Users.Queries.GetUsers;

/// <summary>
///     GetUsersQuery validator
/// </summary>
public class GetUsersQueryValidator : AbstractValidator<UsersQueryParameters>
{
    /// <summary>
    ///     The columns allowed to sort by
    /// </summary>
    private readonly IEnumerable<string> _sortingColumns = new List<string>
    {
        "email",
        "username",
        "yerbamateopinions",
        "shopopinions"
    };

    /// <summary>
    ///     GetUsersQueryValidator
    /// </summary>
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Role)
            .MaximumLength(15);

        RuleFor(x => x.SortBy)
            .Must(value =>
                string.IsNullOrEmpty(value) || _sortingColumns.Contains(value.ToLower()))
            .WithMessage($"SortBy must be in [{string.Join(", ", _sortingColumns)}]");
    }
}