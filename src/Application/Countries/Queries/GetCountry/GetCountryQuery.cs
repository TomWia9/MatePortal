using System;
using MediatR;

namespace Application.Countries.Queries.GetCountry
{
    /// <summary>
    /// Get single country query
    /// </summary>
    public class GetCountryQuery : IRequest<CountryDto>
    {
        /// <summary>
        /// Initializes GetCountryQuery
        /// </summary>
        /// <param name="countryId">Country ID</param>
        public GetCountryQuery(Guid countryId)
        {
            CountryId = countryId;
        }

        /// <summary>
        /// Country ID
        /// </summary>
        private Guid CountryId { get; }
    }
}