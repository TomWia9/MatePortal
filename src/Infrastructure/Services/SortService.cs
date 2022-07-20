using System;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;
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
    /// <param name="exp">Sorting expression</param>
    /// <param name="sortDirection">Sorting direction</param>
    /// <returns>Sorted collection of type IQueryable</returns>
    public IQueryable<T> Sort(IQueryable<T> collection, Expression<Func<T, object>> exp, SortDirection sortDirection)
    {
        collection = sortDirection == SortDirection.Asc
            ? collection.OrderBy(exp)
            : collection.OrderByDescending(exp);

        return collection;
    }
}