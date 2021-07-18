using System;
using MediatR;

namespace Application.Favourites.Commands.DeleteFavourite
{
    public class DeleteFavouriteCommand : IRequest
    {
        public Guid Id { get; set; } 
    }
}