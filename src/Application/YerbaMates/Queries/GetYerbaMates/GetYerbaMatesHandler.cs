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
    ///     Sort service
    /// </summary>
    private readonly ISortService<YerbaMate> _sortService;

    /// <summary>
    ///     Initializes GetYerbaMatesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="sortService">Sort service</param>
    public GetYerbaMatesHandler(IApplicationDbContext context,
        IMapper mapper,
        ISortService<YerbaMate> sortService)
    {
        _context = context;
        _mapper = mapper;
        _sortService = sortService;
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

        //filtering
        var predicates = new List<Expression<Func<YerbaMate, bool>>>
        {
            request.Parameters.Brand != null ? y => y.Brand.Name == request.Parameters.Brand : null,
            request.Parameters.Country != null ? y => y.Brand.Country.Name == request.Parameters.Country : null,
            request.Parameters.Category != null ? y => y.Category.Name == request.Parameters.Category : null,
            request.Parameters.MaxPrice != null ? y => y.AveragePrice <= request.Parameters.MaxPrice : null
        };
        
        collection = Filter(collection, predicates);
        
        //searching
        if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
        {
            var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();

            collection = collection.Where(y => y.Name.ToLower().Contains(searchQuery)
                                               || y.Description.ToLower().Contains(searchQuery)
                                               || y.Brand.Name.ToLower().Contains(searchQuery)
                                               || y.Brand.Country.Name.ToLower().Contains(searchQuery)
                                               || y.Category.Name.ToLower().Contains(searchQuery));
        }

        //sorting
        if (!string.IsNullOrWhiteSpace(request.Parameters.SortBy))
        {
            var sortingColumns = new Dictionary<string, Expression<Func<YerbaMate, object>>>
            {
                {nameof(YerbaMate.Name).ToLower(), y => y.Name},
                {nameof(YerbaMate.AveragePrice).ToLower(), y => y.AveragePrice},
                {nameof(YerbaMate.YerbaMateOpinions).ToLower(), y => y.YerbaMateOpinions.Count},
                {nameof(YerbaMate.Favourites).ToLower(), y => y.Favourites.Count}
            };

            collection = _sortService.Sort(collection, request.Parameters.SortBy,
                request.Parameters.SortDirection, sortingColumns);
        }
        else
        {
            collection = collection.OrderBy(y => y.Name);
        }

        return await collection.ProjectTo<YerbaMateDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Filters collection bt given predicates
    /// </summary>
    /// <param name="collection">The Queryable collection</param>
    /// <param name="predicates">The predicates</param>
    /// <typeparam name="T">The entity type</typeparam>
    private static IQueryable<T> Filter<T>(IQueryable<T> collection, //maybe Search will be more fitting name, this method will also handle searching
        IEnumerable<Expression<Func<T, bool>>> predicates)
    {
        return predicates.Where(predicate => predicate != null)
            .Aggregate(collection, (current, predicate) => current.Where(predicate));
    }
}