using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Shop : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<ShopOpinion> Opinions { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}