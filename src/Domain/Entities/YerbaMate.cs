using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class YerbaMate : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; init; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string imgUrl { get; set; }
        public decimal AveragePrice { get; set; }
        
        //Todo remove numberOfAddToFav
        public int NumberOfAddToFav { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public IList<Opinion> Opinions { get; set; } = new List<Opinion>();
        public IList<Favourite> Favourites { get; set; } = new List<Favourite>();

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}