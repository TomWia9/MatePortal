using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;

namespace Application.Common.Interfaces;

/// <summary>
///     Sort service interface
/// </summary>
/// <typeparam name="T">The type of sorted collection</typeparam>
public interface ISortService<T>
    where T : class
{
    /// <summary>
    ///     Sorts the given collection on the given property in the specified direction
    /// </summary>
    /// <param name="collection">Collection to sort</param>
    /// <param name="exp">Sorting expression</param>
    /// <param name="sortDirection">Sorting direction</param>
    /// <returns>Sorted collection of type IQueryable</returns>
    IQueryable<T> Sort(IQueryable<T> collection, Expression<Func<T, object>> exp, SortDirection sortDirection);
}