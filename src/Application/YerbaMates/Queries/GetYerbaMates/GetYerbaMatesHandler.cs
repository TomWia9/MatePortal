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

namespace Application.YerbaMates.Queries.GetYerbaMates
{
    /// <summary>
    /// Get yerba mates handler
    /// </summary>
    public class GetYerbaMatesHandler : IRequestHandler<GetYerbaMatesQuery, PaginatedList<YerbaMateDto>>
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
        private readonly ISortService<YerbaMate> _sortService;

        /// <summary>
        /// Initializes GetYerbaMatesHandler
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
        /// Handles getting yerba mates
        /// </summary>
        /// <param name="request">Get yerba mates request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of yerba mates data transfer objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
        public async Task<PaginatedList<YerbaMateDto>> Handle(GetYerbaMatesQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Parameters == null)
            {
                throw new ArgumentNullException(nameof(request.Parameters));
            }

            var collection = _context.YerbaMate
                .Include(y => y.Brand)
                .Include(y => y.Category)
                .Include(y => y.Opinions)
                .Include(y => y.Favourites).AsQueryable();

            //filtering
            if (request.Parameters.Brand != null)
            {
                collection = collection.Where(y => y.Brand.Name == request.Parameters.Brand);
            }

            if (request.Parameters.Country != null)
            {
                collection = collection.Where(y => y.Brand.Country.Name == request.Parameters.Country);
            }

            if (request.Parameters.Category != null)
            {
                collection = collection.Where(y => y.Category.Name == request.Parameters.Category);
            }

            if (request.Parameters.MaxPrice != null)
            {
                collection = collection.Where(y => y.AveragePrice <= request.Parameters.MaxPrice);
            }


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
                    { nameof(YerbaMate.Name), y => y.Name },
                    { nameof(YerbaMate.AveragePrice), y => y.AveragePrice },
                    { nameof(YerbaMate.Opinions), y => y.Opinions.Count },
                    { nameof(YerbaMate.Favourites), y => y.Favourites.Count },
                };

                collection = _sortService.Sort(collection, request.Parameters.SortBy,
                    request.Parameters.SortDirection, sortingColumns);

                return await collection.ProjectTo<YerbaMateDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
            }

            //If sortBy is null, sort by name
            return await collection.OrderBy(b => b.Name).ProjectTo<YerbaMateDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
        }
    }
}