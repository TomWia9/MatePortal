using System;
using Application.Favourites.Queries;
using MediatR;

namespace Application.Favourites.Commands.CreateFavourite
{
    /// <summary>
    /// Create favourite command
    /// </summary>
    public class CreateFavourite : IRequest<FavouriteDto>
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; set; }
    }
}