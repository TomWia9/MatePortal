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

namespace Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;

/// <summary>
///     Get yerba mate opinions handler
/// </summary>
public class GetYerbaMateOpinionsHandler : IRequestHandler<GetYerbaMateOpinionsQuery, PaginatedList<YerbaMateOpinionDto>>
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
    private readonly ISortService<YerbaMateOpinion> _sortService;

    /// <summary>
    ///     Initializes GetYerbaMateOpinionsHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="sortService">Sort service</param>
    public GetYerbaMateOpinionsHandler(IApplicationDbContext context,
        IMapper mapper,
        ISortService<YerbaMateOpinion> sortService)
    {
        _context = context;
        _mapper = mapper;
        _sortService = sortService;
    }

    /// <summary>
    ///     Handles getting yerba mate opinions
    /// </summary>
    /// <param name="request">Get yerba mate opinions request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of opinion data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<YerbaMateOpinionDto>> Handle(GetYerbaMateOpinionsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.YerbaMateOpinions.Where(o => o.YerbaMateId == request.YerbaMateId).AsQueryable();

        //filtering
        collection = collection.Where(o =>
            o.Rate >= request.Parameters.MinRate && o.Rate <= request.Parameters.MaxRate);

        //searching
        if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
        {
            var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();

            collection = collection.Where(o => o.Comment.ToLower().Contains(searchQuery));
        }

        //sorting
        if (!string.IsNullOrWhiteSpace(request.Parameters.SortBy))
        {
            var sortingColumns = new Dictionary<string, Expression<Func<YerbaMateOpinion, object>>>
            {
                {nameof(YerbaMateOpinion.Created).ToLower(), o => o.Created},
                {nameof(YerbaMateOpinion.Comment).ToLower(), o => o.Comment},
                {nameof(YerbaMateOpinion.Rate).ToLower(), o => o.Rate}
            };

            collection = _sortService.Sort(collection, request.Parameters.SortBy,
                request.Parameters.SortDirection, sortingColumns);
        }
        else
        {
            collection = collection.OrderBy(o => o.Created);
        }

        return await collection.ProjectTo<YerbaMateOpinionDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
}