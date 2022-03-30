using System;
using Application.Common.Models;
using MediatR;

namespace Application.ShopOpinions.Queries.GetShopOpinions
{
    /// <summary>
    /// Get all opinions about single shop query
    /// </summary>
    public class GetShopOpinionsQuery : IRequest<PaginatedList<ShopOpinionDto>>
    {
        /// <summary>
        /// Initializes GetShopOpinionsQuery
        /// </summary>
        /// <param name="shopId">Shop ID from which opinions can be obtained</param>
        /// <param name="parameters">Shop opinions query parameters</param>
        public GetShopOpinionsQuery(Guid shopId, ShopOpinionsQueryParameters parameters)
        {
            ShopId = shopId;
            Parameters = parameters;
        }

        /// <summary>
        /// Shop ID from which opinions can be obtained
        /// </summary>
        public Guid ShopId { get; }

        /// <summary>
        /// Shop opinions query parameters
        /// </summary>
        public ShopOpinionsQueryParameters Parameters { get; }
    }
}