using System;
using Application.Common.Models;
using MediatR;

namespace Application.ShopOpinions.Queries.GetUserShopOpinions
{
    /// <summary>
    /// Get all opinions about shops posted by single user query
    /// </summary>
    public class GetUserShopOpinionsQuery : IRequest<PaginatedList<ShopOpinionDto>>
    {
        /// <summary>
        /// Initializes GetUserShopOpinionsQuery
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="parameters">Shop opinions query parameters</param>
        public GetUserShopOpinionsQuery(Guid userId, ShopOpinionsQueryParameters parameters)
        {
            UserId = userId;
            Parameters = parameters;
        }

        /// <summary>
        /// User ID
        /// </summary>
        private Guid UserId { get; }
        
        /// <summary>
        /// Shop opinions query parameters
        /// </summary>
        private ShopOpinionsQueryParameters Parameters { get; }
    }
}