using Application.Common.Extensions;
using Application.Common.QueryParameters;

namespace Application.Brands.Queries
{
    public class BrandsQueryParameters : QueryParameters
    {
        private string _country;

        public string Country
        {
            get => string.IsNullOrEmpty(_country) ? _country : _country.FirstCharToUpper();
            set => _country = value;
        }
    }
}