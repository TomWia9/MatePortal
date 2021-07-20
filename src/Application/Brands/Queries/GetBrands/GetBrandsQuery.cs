using Application.Common.Models;
using MediatR;

namespace Application.Brands.Queries.GetBrands
{
    /// <summary>
    /// Get all brands query
    /// </summary>
    public class GetBrandsQuery : IRequest<PaginatedList<BrandDto>>
    {
        /// <summary>
        /// Initializes GetBrandsQuery
        /// </summary>
        /// <param name="parameters">Brands query parameters</param>
        public GetBrandsQuery(BrandsQueryParameters parameters)
        {
            Parameters = parameters;
        }
        
        /// <summary>
        /// Brands query parameters
        /// </summary>
        private BrandsQueryParameters Parameters { get; }
    }
}