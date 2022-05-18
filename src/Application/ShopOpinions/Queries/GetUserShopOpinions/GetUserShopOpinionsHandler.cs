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

namespace Application.ShopOpinions.Queries.GetUserShopOpinions
{
    /// <summary>
    ///     Get user's shop opinions handler
    /// </summary>
    public class
        GetUserShopOpinionsHandler : IRequestHandler<GetUserShopOpinionsQuery, PaginatedList<ShopOpinionDto>>
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
        private readonly ISortService<ShopOpinion> _sortService;

        /// <summary>
        ///     Initializes GetUserShopOpinionsHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="sortService">Sort service</param>
        public GetUserShopOpinionsHandler(IApplicationDbContext context,
            IMapper mapper,
            ISortService<ShopOpinion> sortService)
        {
            _context = context;
            _mapper = mapper;
            _sortService = sortService;
        }

        /// <summary>
        ///     Handles getting user's shop opinions
        /// </summary>
        /// <param name="request">Get user's shop opinions request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of shop opinion data transfer objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
        public async Task<PaginatedList<ShopOpinionDto>> Handle(GetUserShopOpinionsQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

            var collection = _context.ShopOpinions.AsQueryable();

            //filtering
            collection = collection.Where(o => o.CreatedBy == request.UserId &&
                                               o.Rate >= request.Parameters.MinRate &&
                                               o.Rate <= request.Parameters.MaxRate);

            //searching
            if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
            {
                var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();

                collection = collection.Where(o => o.Comment.ToLower().Contains(searchQuery));
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(request.Parameters.SortBy))
            {
                var sortingColumns = new Dictionary<string, Expression<Func<ShopOpinion, object>>>
                {
                    {nameof(Opinion.Created), o => o.Created},
                    {nameof(Opinion.Comment), o => o.Comment},
                    {nameof(Opinion.Rate), o => o.Rate}
                };

                collection = _sortService.Sort(collection, request.Parameters.SortBy,
                    request.Parameters.SortDirection, sortingColumns);

                return await collection.ProjectTo<ShopOpinionDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
            }

            //If sortBy is null, sort by created date
            return await collection.OrderBy(o => o.Created).ProjectTo<ShopOpinionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
        }
    }
}