using System;
using MediatR;

namespace Application.ShopOpinions.Queries.GetShopOpinion
{
    /// <summary>
    /// Get single opinion about shop query
    /// </summary>
    public class GetShopOpinionQuery : IRequest<ShopOpinionDto>
    {
        /// <summary>
        /// Initializes GetShopOpinionQuery
        /// </summary>
        /// <param name="shopOpinionId">Shop opinion ID</param>
        public GetShopOpinionQuery(Guid shopOpinionId)
        {
            ShopOpinionId = shopOpinionId;
        }

        /// <summary>
        /// Shop opinion ID
        /// </summary>
        private Guid ShopOpinionId { get; }
    }
}