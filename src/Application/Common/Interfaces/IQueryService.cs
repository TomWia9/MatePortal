using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;

namespace Application.Common.Interfaces;

/// <summary>
///     Query service interface
/// </summary>
/// <typeparam name="T">The type of the entity</typeparam>
public interface IQueryService<T>
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

    /// <summary>
    ///     Searches collection by given predicates
    /// </summary>
    /// <param name="collection">The Queryable collection</param>
    /// <param name="predicates">The predicates</param>
    /// <typeparam name="T">The entity type</typeparam>
    IQueryable<T> Search(IQueryable<T> collection,
        IEnumerable<Expression<Func<T, bool>>> predicates);
}