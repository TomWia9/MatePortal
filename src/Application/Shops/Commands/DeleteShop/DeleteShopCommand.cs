using System;
using MediatR;

namespace Application.Shops.Commands.DeleteShop
{
    public class DeleteShopCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}