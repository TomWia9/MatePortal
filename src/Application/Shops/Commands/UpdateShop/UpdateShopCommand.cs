using System;
using MediatR;

namespace Application.Shops.Commands.UpdateShop
{
    public class UpdateShopCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}