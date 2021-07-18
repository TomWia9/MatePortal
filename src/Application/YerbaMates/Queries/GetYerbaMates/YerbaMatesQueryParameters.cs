using Application.Common.Extensions;
using Application.Common.QueryParameters;

namespace Application.YerbaMates.Queries.GetYerbaMates
{
    public class YerbaMatesQueryParameters : QueryParameters
    {
        private string _country;
        private string _brand;
        private string _category;

        public string Country
        {
            get => string.IsNullOrEmpty(_country) ? _country : _country.FirstCharToUpper();
            set => _country = value;
        }
        
        public string Brand
        {
            get => string.IsNullOrEmpty(_brand) ? _brand : _brand.FirstCharToUpper();
            set => _brand = value;
        }

        public string Category
        {
            get => string.IsNullOrEmpty(_category) ? _category : _category.FirstCharToUpper();
            set => _category = value;
        }

        public decimal MaxPrice { get; set; }
    }
}