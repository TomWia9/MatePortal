using Application.Common.Models;
using MediatR;

namespace Application.Countries.Queries.GetCountries;

/// <summary>
///     Get countries query
/// </summary>
public class GetCountriesQuery : IRequest<PaginatedList<CountryDto>>
{
    /// <summary>
    ///     Initializes GetCountriesQuery
    /// </summary>
    /// <param name="parameters">Countries query parameters</param>
    public GetCountriesQuery(CountriesQueryParameters parameters)
    {
        Parameters = parameters;
    }

    public CountriesQueryParameters Parameters { get; }
}