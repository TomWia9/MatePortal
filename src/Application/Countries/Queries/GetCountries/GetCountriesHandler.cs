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

namespace Application.Countries.Queries.GetCountries;

/// <summary>
///     Get countries handler
/// </summary>
public class GetCountriesHandler : IRequestHandler<GetCountriesQuery, PaginatedList<CountryDto>>
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
    ///     The query service
    /// </summary>
    private readonly IQueryService<Country> _queryService;

    /// <summary>
    ///     Initializes GetCountriesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="queryService">The query service</param>
    public GetCountriesHandler(IApplicationDbContext context, IMapper mapper, IQueryService<Country> queryService)
    {
        _context = context;
        _mapper = mapper;
        _queryService = queryService;
    }

    /// <summary>
    ///     Handles getting countries
    /// </summary>
    /// <param name="request">Get countries request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of country data transfer objects</returns>
    public async Task<PaginatedList<CountryDto>> Handle(GetCountriesQuery request,
        CancellationToken cancellationToken)
    {
        var collection = _context.Countries.AsQueryable();

        var predicates = GetPredicates(request.Parameters);
        var sortingColumn = GetSortingColumn(request.Parameters.SortBy);

        collection = _queryService.Search(collection, predicates);
        collection = _queryService.Sort(collection, sortingColumn, request.Parameters.SortDirection);

        return await collection.ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }

    /// <summary>
    ///     Gets filtering and searching predicates for countries
    /// </summary>
    /// <param name="parameters">The countries query parameters</param>
    /// <returns>The countries predicates</returns>
    private static IEnumerable<Expression<Func<Country, bool>>> GetPredicates(CountriesQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<Country, bool>>>();

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates;

        var searchQuery = parameters.SearchQuery.Trim().ToLower();

        Expression<Func<Country, bool>> searchPredicate =
            x => x.Name.ToLower().Contains(searchQuery) ||
                 x.Description.ToLower().Contains(searchQuery);

        predicates.Add(searchPredicate);

        return predicates;
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<Country, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<Country, object>>>
        {
            {nameof(Country.Name).ToLower(), x => x.Name},
            {nameof(Country.Description).ToLower(), x => x.Description},
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }
}