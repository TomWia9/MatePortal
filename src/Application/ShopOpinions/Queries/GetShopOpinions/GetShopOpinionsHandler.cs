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

namespace Application.ShopOpinions.Queries.GetShopOpinions;

/// <summary>
///     Get shop opinions handler
/// </summary>
public class GetShopOpinionsHandler : IRequestHandler<GetShopOpinionsQuery, PaginatedList<ShopOpinionDto>>
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
    private readonly IQueryService<ShopOpinion> _queryService;

    /// <summary>
    ///     Initializes GetShopOpinionsHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">Query service</param>
    public GetShopOpinionsHandler(IApplicationDbContext context,
        IMapper mapper,
        IQueryService<ShopOpinion> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
    }

    /// <summary>
    ///     Handles getting shop opinions
    /// </summary>
    /// <param name="request">Get shop opinions request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of shop opinion data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<ShopOpinionDto>> Handle(GetShopOpinionsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.ShopOpinions.Where(o => o.ShopId == request.ShopId).AsQueryable();

        var predicates = GetPredicates(request.Parameters);
        var sortingColumn = GetSortingColumn(request.Parameters.SortBy);

        collection = _queryService.Search(collection, predicates);
        collection = _queryService.Sort(collection, sortingColumn, request.Parameters.SortDirection);

        return await collection.ProjectTo<ShopOpinionDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Gets filtering and searching predicates for shop opinions
    /// </summary>
    /// <param name="parameters">The shop opinions query parameters</param>
    /// <returns>The shop opinions predicates</returns>
    private static IEnumerable<Expression<Func<ShopOpinion, bool>>> GetPredicates(
        ShopOpinionsQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<ShopOpinion, bool>>>
        {
            x => x.Rate >= parameters.MinRate && x.Rate <= parameters.MaxRate
        };

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates.Where(x => x != null);

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        predicates.Add(x => x.Comment.ToLower().Contains(searchQuery));

        return predicates.Where(x => x != null);
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<ShopOpinion, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<ShopOpinion, object>>>
        {
            {nameof(ShopOpinion.Created).ToLower(), x => x.Created},
            {nameof(ShopOpinion.Comment).ToLower(), x => x.Comment},
            {nameof(ShopOpinion.Rate).ToLower(), x => x.Rate}
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }
}