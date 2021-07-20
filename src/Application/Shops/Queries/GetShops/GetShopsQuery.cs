using Application.Common.Models;
using MediatR;

namespace Application.Shops.Queries.GetShops
{
    /// <summary>
    /// Get all shops query
    /// </summary>
    public class GetShopsQuery : IRequest<PaginatedList<ShopDto>>
    {
        /// <summary>
        /// Initializes GetShopsQuery
        /// </summary>
        /// <param name="parameters">Shops query parameters</param>
        public GetShopsQuery(ShopsQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Shops query parameters
        /// </summary>
        private ShopsQueryParameters Parameters { get; }
    }
}