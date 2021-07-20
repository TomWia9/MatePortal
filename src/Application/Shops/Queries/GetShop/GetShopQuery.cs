using System;
using MediatR;

namespace Application.Shops.Queries.GetShop
{
    /// <summary>
    /// Get single shop query
    /// </summary>
    public class GetShopQuery : IRequest<ShopDto>
    {
        /// <summary>
        /// Initializes GetShopQuery
        /// </summary>
        /// <param name="shopId">Shop ID</param>
        public GetShopQuery(Guid shopId)
        {
            ShopId = shopId;
        }

        /// <summary>
        /// Shop ID
        /// </summary>
        private Guid ShopId { get; }
    }
}