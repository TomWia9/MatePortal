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

namespace Application.Shops.Queries.GetShops
{
    /// <summary>
    /// Get shops handler
    /// </summary>
    public class GetShopsHandler : IRequestHandler<GetShopsQuery, PaginatedList<ShopDto>>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Sort service
        /// </summary>
        private readonly ISortService<Shop> _sortService;

        /// <summary>
        /// Initializes GetShopsHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="sortService">Sort service</param>
        public GetShopsHandler(IApplicationDbContext context,
            IMapper mapper,
            ISortService<Shop> sortService)
        {
            _context = context;
            _mapper = mapper;
            _sortService = sortService;
        }

        /// <summary>
        /// Handles getting shops
        /// </summary>
        /// <param name="request">Get shops request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of shop data transfer objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
        public async Task<PaginatedList<ShopDto>> Handle(GetShopsQuery request, CancellationToken cancellationToken)
        {
            if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

            var collection = _context.Shops.Include(s => s.Opinions) as IQueryable<Shop>;

            //searching
            if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
            {
                var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();

                collection = collection.Where(s => s.Name.ToLower().Contains(searchQuery)
                                                   || s.Description.ToLower().Contains(searchQuery));
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(request.Parameters.SortBy))
            {
                var sortingColumns = new Dictionary<string, Expression<Func<Shop, object>>>
                {
                    { nameof(Shop.Name), s => s.Name },
                    { nameof(Shop.Description), s => s.Description },
                    { nameof(Shop.Opinions), y => y.Opinions.Count }
                };

                collection = _sortService.Sort(collection, request.Parameters.SortBy,
                    request.Parameters.SortDirection, sortingColumns);

                return await collection.ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
            }

            //If sortBy is null, sort by name
            return await collection.OrderBy(b => b.Name).ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
        }
    }
}