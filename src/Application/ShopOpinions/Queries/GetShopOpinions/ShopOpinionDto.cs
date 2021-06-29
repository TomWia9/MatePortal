using System;

namespace Application.ShopOpinions.Queries.GetShopOpinions
{
    public class ShopOpinionDto
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid ShopId { get; set; }
        public string User { get; set; }
    }
}