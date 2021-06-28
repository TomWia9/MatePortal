using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class YerbaMate : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string imgUrl { get; set; }
        public decimal AveragePrice { get; set; }
        public int NumberOfAddToFav { get; set; }

        public List<Category> Categories { get; set; }
        public List<Opinion> Opinions { get; set; }
        public List<Favourite> Favourites { get; set; }

        public List<DomainEvent> DomainEvents { get; set; }
    }
}