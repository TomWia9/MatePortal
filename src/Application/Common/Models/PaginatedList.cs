using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models;

/// <summary>
///     Paginated list
/// </summary>
/// <typeparam name="T">Type of the paginated list</typeparam>
public class PaginatedList<T> : List<T>
{
    /// <summary>
    ///     Initializes paginated list
    /// </summary>
    /// <param name="items">The items</param>
    /// <param name="count">The items count</param>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        AddRange(items);
    }

    /// <summary>
    ///     The current page
    /// </summary>
    public int CurrentPage { get; }

    /// <summary>
    ///     The total number of pages
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    ///     The page size
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    ///     The total count of items
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    ///     Checks that list has a previous page
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    ///     Checks that the list has a next page
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;

    /// <summary>
    ///     Converts queryable to the paginated list
    /// </summary>
    /// <param name="source">The source with specified type</param>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <returns>Paginated list of specified type</returns>
    public static async Task<PaginatedList<T>> ToPaginatedListAsync(IQueryable<T> source, int pageNumber,
        int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}