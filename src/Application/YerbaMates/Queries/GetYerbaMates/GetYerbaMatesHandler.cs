using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Enums;
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
    ///     Sort service
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
            .Include(y => y.Brand).AsQueryable()
            .Include(y => y.Category).AsQueryable()
            .Include(y => y.YerbaMateOpinions).AsQueryable()
            .Include(y => y.Favourites).AsQueryable();

        var predicates = GetPredicates(request.Parameters);

        collection = _queryService.Search(collection, predicates);
        collection = Sort(collection, request.Parameters.SortBy, request.Parameters.SortDirection);

        return await collection.ProjectTo<YerbaMateDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    /// Gets filtering and searching predicates for yerba mates
    /// </summary>
    /// <param name="parameters">The yerba mate query parameters</param>
    /// <returns>The yerba mate predicates</returns>
    private static IEnumerable<Expression<Func<YerbaMate, bool>>> GetPredicates(YerbaMatesQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<YerbaMate, bool>>>
        {
            !string.IsNullOrWhiteSpace(parameters.Brand) ? y => y.Brand.Name == parameters.Brand : null,
            !string.IsNullOrWhiteSpace(parameters.Country) ? y => y.Brand.Country.Name == parameters.Country : null,
            !string.IsNullOrWhiteSpace(parameters.Category) ? y => y.Category.Name == parameters.Category : null,
            parameters.MaxPrice != null ? y => y.AveragePrice <= parameters.MaxPrice : null
        };

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates.Where(x => x != null);

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        predicates.Add(y => y.Name.ToLower().Contains(searchQuery));
        predicates.Add(y => y.Description.ToLower().Contains(searchQuery));
        predicates.Add(y => y.Brand.Name.ToLower().Contains(searchQuery));
        predicates.Add(y => y.Brand.Country.Name.ToLower().Contains(searchQuery));
        predicates.Add(y => y.Category.Name.ToLower().Contains(searchQuery));

        return predicates.Where(x => x != null);
    }

    /// <summary>
    /// Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<YerbaMate, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<YerbaMate, object>>>
        {
            {nameof(YerbaMate.Name).ToLower(), y => y.Name},
            {nameof(YerbaMate.AveragePrice).ToLower(), y => y.AveragePrice},
            {nameof(YerbaMate.YerbaMateOpinions).ToLower(), y => y.YerbaMateOpinions.Count},
            {nameof(YerbaMate.Favourites).ToLower(), y => y.Favourites.Count}
        };

        return sortingColumns[sortBy];
    }

    /// <summary>
    /// Sorts yerba mates by given column in given direction
    /// </summary>
    /// <param name="collection">The yerba mate collection</param>
    /// <param name="sortBy">Column by which to sort</param>
    /// <param name="sortDirection">Direction in which to sort</param>
    /// <returns>The sorted collection of yerba mates</returns>
    private IQueryable<YerbaMate> Sort(IQueryable<YerbaMate> collection, string sortBy, SortDirection sortDirection)
    {
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var sortingColumn = GetSortingColumn(sortBy);

            collection = _queryService.Sort(collection, sortingColumn, sortDirection);
        }
        else
        {
            collection = collection.OrderBy(y => y.Name);
        }

        return collection;
    }
}