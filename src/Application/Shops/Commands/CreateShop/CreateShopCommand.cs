using Application.Shops.Queries.GetShops;
using MediatR;

namespace Application.Shops.Commands.CreateShop
{
    public class CreateShopCommand : IRequest<ShopDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}