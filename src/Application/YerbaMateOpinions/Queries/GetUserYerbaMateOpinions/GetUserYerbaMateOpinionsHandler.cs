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

namespace Application.YerbaMateOpinions.Queries.GetUserYerbaMateOpinions;

/// <summary>
///     Get user's yerba mate opinions handler
/// </summary>
public class
    GetUserYerbaMateOpinionsHandler : IRequestHandler<GetUserYerbaMateOpinionsQuery, PaginatedList<YerbaMateOpinionDto>>
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
    private readonly IQueryService<YerbaMateOpinion> _queryService;

    /// <summary>
    ///     Initializes GetUsersYerbaMateOpinionsHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">Query service</param>
    public GetUserYerbaMateOpinionsHandler(IApplicationDbContext context,
        IMapper mapper,
        IQueryService<YerbaMateOpinion> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
    }

    /// <summary>
    ///     Handles getting user's yerba mate opinions
    /// </summary>
    /// <param name="request">Get user's yerba mate opinions request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of opinion data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<YerbaMateOpinionDto>> Handle(GetUserYerbaMateOpinionsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.YerbaMateOpinions.AsQueryable();

        collection = collection.Where(o => o.CreatedBy == request.UserId);

        var predicates = GetPredicates(request.Parameters);
        var sortingColumn = GetSortingColumn(request.Parameters.SortBy);

        collection = _queryService.Search(collection, predicates);
        collection = _queryService.Sort(collection, sortingColumn, request.Parameters.SortDirection);

        return await collection.ProjectTo<YerbaMateOpinionDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Gets filtering and searching predicates for yerba mate opinions
    /// </summary>
    /// <param name="parameters">The yerba mate opinions query parameters</param>
    /// <returns>The yerba mate opinions predicates</returns>
    private static IEnumerable<Expression<Func<YerbaMateOpinion, bool>>> GetPredicates(
        YerbaMateOpinionsQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<YerbaMateOpinion, bool>>>
        {
            x => x.Rate >= parameters.MinRate && x.Rate <= parameters.MaxRate
        };

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates;

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        predicates.Add(x => x.Comment.ToLower().Contains(searchQuery));

        return predicates;
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<YerbaMateOpinion, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<YerbaMateOpinion, object>>>
        {
            {nameof(YerbaMateOpinion.Created), x => x.Created},
            {nameof(YerbaMateOpinion.Comment), x => x.Comment},
            {nameof(YerbaMateOpinion.Rate), x => x.Rate}
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }
}