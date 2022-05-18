using Application.Common.Extensions;
using Application.Common.QueryParameters;

namespace Application.Brands.Queries
{
    /// <summary>
    ///     Brands query parameters
    /// </summary>
    public class BrandsQueryParameters : QueryParameters
    {
        private readonly string _country;

        /// <summary>
        ///     Country parameter that indicates from which country the brands should be taken
        /// </summary>
        public string Country
        {
            get => string.IsNullOrEmpty(_country) ? _country : _country.FirstCharToUpper();
            init => _country = value;
        }
    }
}