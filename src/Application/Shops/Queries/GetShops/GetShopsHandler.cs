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
using Microsoft.EntityFrameworkCore;

namespace Application.Shops.Queries.GetShops;

/// <summary>
///     Get shops handler
/// </summary>
public class GetShopsHandler : IRequestHandler<GetShopsQuery, PaginatedList<ShopDto>>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    ///     Query service
    /// </summary>
    private readonly IQueryService<Shop> _queryService;

    /// <summary>
    ///     Initializes GetShopsHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">Query service</param>
    public GetShopsHandler(IApplicationDbContext context,
        IMapper mapper,
        IQueryService<Shop> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
    }

    /// <summary>
    ///     Handles getting shops
    /// </summary>
    /// <param name="request">Get shops request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of shop data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<ShopDto>> Handle(GetShopsQuery request, CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.Shops.Include(s => s.Opinions) as IQueryable<Shop>;

        var predicates = GetPredicates(request.Parameters);
        var sortingColumn = GetSortingColumn(request.Parameters.SortBy);

        collection = _queryService.Search(collection, predicates);
        collection = _queryService.Sort(collection, sortingColumn, request.Parameters.SortDirection);

        return await collection.ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Gets filtering and searching predicates for shops
    /// </summary>
    /// <param name="parameters">The shops query parameters</param>
    /// <returns>The shops predicates</returns>
    private static IEnumerable<Expression<Func<Shop, bool>>> GetPredicates(ShopsQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<Shop, bool>>>();

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates;

        var searchQuery = parameters.SearchQuery.Trim().ToLower();
        Expression<Func<Shop, bool>> searchPredicate =
            x => x.Name.ToLower().Contains(searchQuery) ||
                 x.Description.ToLower().Contains(searchQuery) ||
                 x.Url.ToLower().Contains(searchQuery);

        predicates.Add(searchPredicate);

        return predicates.Where(x => x != null);
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<Shop, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<Shop, object>>>
        {
            {nameof(Shop.Name).ToLower(), x => x.Name},
            {nameof(Shop.Description).ToLower(), x => x.Description},
            {nameof(Shop.Opinions).ToLower(), x => x.Opinions.Count}
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }
}