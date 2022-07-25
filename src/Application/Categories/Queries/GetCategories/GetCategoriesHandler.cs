using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
///     Get categories handler
/// </summary>
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryDto>>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    private readonly IQueryService<Category> _queryService;

    /// <summary>
    ///     Initializes GetCategoriesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">The query service</param>
    public GetCategoriesHandler(IApplicationDbContext context, IMapper mapper, IQueryService<Category> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
    }

    /// <summary>
    ///     Handles getting categories
    /// </summary>
    /// <param name="request">Get categories request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of category data transfer objects</returns>
    public async Task<PaginatedList<CategoryDto>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var collection = _context.Categories.AsQueryable();
        
        var predicates = GetPredicates(request.Parameters);
        var sortingColumn = GetSortingColumn(request.Parameters.SortBy);

        collection = _queryService.Search(collection, predicates);
        collection = _queryService.Sort(collection, sortingColumn, request.Parameters.SortDirection);

        return await collection.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Gets filtering and searching predicates for categories
    /// </summary>
    /// <param name="parameters">The categories query parameters</param>
    /// <returns>The categories predicates</returns>
    private static IEnumerable<Expression<Func<Category, bool>>> GetPredicates(CategoriesQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<Category, bool>>>();

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates;

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        Expression<Func<Category, bool>> searchPredicate =
            x => x.Name.ToLower().Contains(searchQuery) ||
                 x.Description.ToLower().Contains(searchQuery);

        predicates.Add(searchPredicate);

        return predicates;
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<Category, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<Category, object>>>
        {
            {nameof(Category.Name).ToLower(), x => x.Name},
            {nameof(Category.Description).ToLower(), x => x.Description},
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }
}