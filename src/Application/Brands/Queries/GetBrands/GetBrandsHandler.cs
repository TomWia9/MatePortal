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

namespace Application.Brands.Queries.GetBrands
{
    /// <summary>
    ///     Get brands handler
    /// </summary>
    public class GetBrandsHandler : IRequestHandler<GetBrandsQuery, PaginatedList<BrandDto>>
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
        private readonly ISortService<Brand> _sortService;

        /// <summary>
        ///     Initializes GetBrandsHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="sortService">Sort service</param>
        public GetBrandsHandler(IApplicationDbContext context,
            IMapper mapper,
            ISortService<Brand> sortService)
        {
            _context = context;
            _mapper = mapper;
            _sortService = sortService;
        }

        /// <summary>
        ///     Handles getting brands
        /// </summary>
        /// <param name="request">Get brands request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of brand data transfer objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
        public async Task<PaginatedList<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

            var collection = _context.Brands.Include(b => b.Country) as IQueryable<Brand>;

            //filtering
            if (request.Parameters.Country != null)
                collection = collection.Where(b => b.Country.Name == request.Parameters.Country);

            //searching
            if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
            {
                var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();
                
                collection = collection.Where(b => b.Name.ToLower().Contains(searchQuery)
                                                   || b.Description.ToLower().Contains(searchQuery)
                                                   || b.Country.Name.ToLower().Contains(searchQuery));
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(request.Parameters.SortBy))
            {
                var sortingColumns = new Dictionary<string, Expression<Func<Brand, object>>>
                {
                    {nameof(Brand.Name), b => b.Name},
                    {nameof(Brand.Description), b => b.Description},
                    {nameof(Brand.Country), b => b.Country.Name}
                };

                collection = _sortService.Sort(collection, request.Parameters.SortBy,
                    request.Parameters.SortDirection, sortingColumns);
            }
            else
            {
                collection = collection.OrderBy(b => b.Name);
            }

            return await collection.ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
        }
    }
}