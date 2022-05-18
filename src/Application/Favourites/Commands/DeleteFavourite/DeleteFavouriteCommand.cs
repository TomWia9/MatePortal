using System;
using MediatR;

namespace Application.Favourites.Commands.DeleteFavourite
{
    /// <summary>
    ///     Delete favourite command
    /// </summary>
    public class DeleteFavouriteCommand : IRequest
    {
        /// <summary>
        ///     Favourite ID
        /// </summary>
        public Guid FavouriteId { get; init; }
    }
}