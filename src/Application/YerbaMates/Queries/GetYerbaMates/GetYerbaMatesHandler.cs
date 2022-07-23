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

namespace Application.YerbaMates.Queries.GetYerbaMates;

/// <summary>
///     Get yerba mates handler
/// </summary>
public class GetYerbaMatesHandler : IRequestHandler<GetYerbaMatesQuery, PaginatedList<YerbaMateDto>>
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
    private readonly IQueryService<YerbaMate> _queryService;

    /// <summary>
    ///     Initializes GetYerbaMatesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">Query service</param>
    public GetYerbaMatesHandler(IApplicationDbContext context,
        IMapper mapper,
        IQueryService<YerbaMate> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
    }

    /// <summary>
    ///     Handles getting yerba mates
    /// </summary>
    /// <param name="request">Get yerba mates request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of yerba mates data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<YerbaMateDto>> Handle(GetYerbaMatesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.YerbaMate.AsQueryable()
            .Include(x => x.Brand).AsQueryable()
            .Include(x => x.Category).AsQueryable()
            .Include(x => x.YerbaMateOpinions).AsQueryable()
            .Include(x => x.Favourites).AsQueryable();

        var predicates = GetPredicates(request.Parameters);
        var sortingColumn = GetSortingColumn(request.Parameters.SortBy);

        collection = _queryService.Search(collection, predicates);
        collection = _queryService.Sort(collection, sortingColumn, request.Parameters.SortDirection);

        return await collection.ProjectTo<YerbaMateDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Gets filtering and searching predicates for yerba mates
    /// </summary>
    /// <param name="parameters">The yerba mate query parameters</param>
    /// <returns>The yerba mate predicates</returns>
    private static IEnumerable<Expression<Func<YerbaMate, bool>>> GetPredicates(YerbaMatesQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<YerbaMate, bool>>>
        {
            !string.IsNullOrWhiteSpace(parameters.Brand) ? x => x.Brand.Name.ToLower() == parameters.Brand : null,
            !string.IsNullOrWhiteSpace(parameters.Country)
                ? x => x.Brand.Country.Name.ToLower() == parameters.Country
                : null,
            !string.IsNullOrWhiteSpace(parameters.Category)
                ? x => x.Category.Name.ToLower() == parameters.Category
                : null,
            parameters.MaxPrice != null ? x => x.AveragePrice <= parameters.MaxPrice : null
        };

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates.Where(x => x != null);

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        Expression<Func<YerbaMate, bool>> searchPredicate =
            x => x.Name.ToLower().Contains(searchQuery) ||
                 x.Description.ToLower().Contains(searchQuery) ||
                 x.Brand.Name.ToLower().Contains(searchQuery) ||
                 x.Brand.Country.Name.ToLower().Contains(searchQuery) ||
                 x.Category.Name.ToLower().Contains(searchQuery);

        predicates.Add(searchPredicate);

        return predicates.Where(x => x != null);
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<YerbaMate, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<YerbaMate, object>>>
        {
            {nameof(YerbaMate.Name).ToLower(), x => x.Name},
            {nameof(YerbaMate.AveragePrice).ToLower(), x => x.AveragePrice},
            {nameof(YerbaMate.YerbaMateOpinions).ToLower(), x => x.YerbaMateOpinions.Count},
            {nameof(YerbaMate.Favourites).ToLower(), x => x.Favourites.Count}
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }
}