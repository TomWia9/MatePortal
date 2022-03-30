using System;
using Application.Favourites.Queries;
using MediatR;

namespace Application.Favourites.Commands.CreateFavourite
{
    /// <summary>
    /// Create favourite command
    /// </summary>
    public class CreateFavouriteCommand : IRequest<FavouriteDto>
    {
        /// <summary>
        /// Yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; set; }
    }
}