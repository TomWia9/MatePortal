using Application.Common.Extensions;
using Application.Common.QueryParameters;

namespace Application.YerbaMates.Queries
{
    /// <summary>
    /// Yerba mates query parameters
    /// </summary>
    public class YerbaMatesQueryParameters : QueryParameters
    {
        private string _country;
        private string _brand;
        private string _category;

        /// <summary>
        /// Country parameter that indicates from which country yerba mates should be taken
        /// </summary>
        public string Country
        {
            get => string.IsNullOrEmpty(_country) ? _country : _country.FirstCharToUpper();
            set => _country = value;
        }

        /// <summary>
        /// Brand parameter that indicates from which brand yerba mates should be taken
        /// </summary>
        public string Brand
        {
            get => string.IsNullOrEmpty(_brand) ? _brand : _brand.FirstCharToUpper();
            set => _brand = value;
        }

        /// <summary>
        /// Category parameter that indicates from which category yerba mates should be taken
        /// </summary>
        public string Category
        {
            get => string.IsNullOrEmpty(_category) ? _category : _category.FirstCharToUpper();
            set => _category = value;
        }

        /// <summary>
        /// Maximum price parameter
        /// </summary>
        public decimal MaxPrice { get; set; }
    }
}