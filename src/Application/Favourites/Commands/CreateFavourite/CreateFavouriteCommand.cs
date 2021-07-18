using System;
using Application.Favourites.Queries.GetFavourites;
using MediatR;

namespace Application.Favourites.Commands.CreateFavourite
{
    public class CreateFavourite : IRequest<FavouriteDto>
    {
        public Guid UserId { get; set; }
        public Guid YerbaMateId { get; set; }
    }
}