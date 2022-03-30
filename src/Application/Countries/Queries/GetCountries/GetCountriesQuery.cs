using System.Collections.Generic;
using MediatR;

namespace Application.Countries.Queries.GetCountries
{
    /// <summary>
    /// Get all countries query
    /// </summary>
    public class GetCountriesQuery : IRequest<IEnumerable<CountryDto>>
    {
    }
}