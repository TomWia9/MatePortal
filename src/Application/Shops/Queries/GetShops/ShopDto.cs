using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Shops.Queries.GetShops
{
    public class ShopDto : IMapFrom<Shop>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}