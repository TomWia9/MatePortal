﻿using System;
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

namespace Application.Brands.Queries.GetBrands;

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
    ///     Query service
    /// </summary>
    private readonly IQueryService<Brand> _queryService;

    /// <summary>
    ///     Initializes GetBrandsHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">Sort service</param>
    public GetBrandsHandler(IApplicationDbContext context,
        IMapper mapper,
        IQueryService<Brand> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
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

        var predicates = GetPredicates(request.Parameters);

        collection = _queryService.Search(collection, predicates);
        collection = Sort(collection, request.Parameters.SortBy, request.Parameters.SortDirection);

        return await collection.ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
    
    /// <summary>
    ///     Gets filtering and searching predicates for the brands
    /// </summary>
    /// <param name="parameters">The brands query parameters</param>
    /// <returns>The brands predicates</returns>
    private static IEnumerable<Expression<Func<Brand, bool>>> GetPredicates(BrandsQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<Brand, bool>>>
        {
            !string.IsNullOrWhiteSpace(parameters.Country) ? x => x.Country.Name == parameters.Country : null
        };

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates.Where(x => x != null);

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        predicates.Add(x => x.Name.ToLower().Contains(searchQuery));
        predicates.Add(x => x.Description.ToLower().Contains(searchQuery));
        predicates.Add(x => x.Country.Name.ToLower().Contains(searchQuery));

        return predicates.Where(x => x != null);
    }
    
    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<Brand, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<Brand, object>>>
        {
            {nameof(Brand.Name).ToLower(), x => x.Name},
            {nameof(Brand.Description).ToLower(), x => x.Description},
            {nameof(Brand.Country).ToLower(), x => x.Country.Name}
        };

        return sortingColumns[sortBy];
    }

    /// <summary>
    ///     Sorts brands by given column in given direction
    /// </summary>
    /// <param name="collection">The brands collection</param>
    /// <param name="sortBy">Column by which to sort</param>
    /// <param name="sortDirection">Direction in which to sort</param>
    /// <returns>The sorted collection of the brands</returns>
    private IQueryable<Brand> Sort(IQueryable<Brand> collection, string sortBy, SortDirection sortDirection)
    {
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var sortingColumn = GetSortingColumn(sortBy);

            collection = _queryService.Sort(collection, sortingColumn, sortDirection);
        }
        else
        {
            collection = collection.OrderBy(x => x.Name);
        }

        return collection;
    }
}