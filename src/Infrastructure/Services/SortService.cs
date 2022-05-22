﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;
using Application.Common.Extensions;
using Application.Common.Interfaces;

namespace Infrastructure.Services;

/// <summary>
///     Sort service
/// </summary>
/// <typeparam name="T">The type of sorted collection</typeparam>
public class SortService<T> : ISortService<T>
    where T : class
{
    /// <summary>
    ///     Sorts the given collection on the given property in the specified direction
    /// </summary>
    /// <param name="collection">Collection to sort</param>
    /// <param name="sortBy">Property by which to sort</param>
    /// <param name="sortDirection">Sorting direction</param>
    /// <param name="sortingColumns">Columns allowed to be sorted by</param>
    /// <returns>Sorted collection of type IQueryable</returns>
    public IQueryable<T> Sort(IQueryable<T> collection, string sortBy, SortDirection sortDirection,
        Dictionary<string, Expression<Func<T, object>>> sortingColumns)
    {
        var selectedColumn = sortingColumns[sortBy.FirstCharToUpper()];

        collection = sortDirection == SortDirection.Asc
            ? collection.OrderBy(selectedColumn)
            : collection.OrderByDescending(selectedColumn);

        return collection;
    }
}