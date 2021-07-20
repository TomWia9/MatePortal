using System;
using Application.Common.Models;
using MediatR;

namespace Application.Favourites.Queries.GetFavourites
{
    /// <summary>
    /// Get user favourites yerba mates query
    /// </summary>
    public class GetFavouritesQuery : IRequest<PaginatedList<FavouriteDto>>
    {
        /// <summary>
        /// Initializes GetFavouritesQuery
        /// </summary>
        /// <param name="userId">User ID</param>
        public GetFavouritesQuery(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User ID
        /// </summary>
        private Guid UserId { get; }
    }
}