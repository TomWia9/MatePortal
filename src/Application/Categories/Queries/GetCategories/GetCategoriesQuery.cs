using System.Collections.Generic;
using Application.Common.Models;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
///     Get all categories query
/// </summary>
public class GetCategoriesQuery : IRequest<PaginatedList<CategoryDto>>
{
    /// <summary>
    ///     Initializes GetCategoriesQuery
    /// </summary>
    /// <param name="parameters">Users query parameters</param>
    public GetCategoriesQuery(CategoriesQueryParameters parameters)
    {
        Parameters = parameters;
    }
    
    /// <summary>
    ///     Categories query parameters
    /// </summary>
    public CategoriesQueryParameters Parameters { get; }
}