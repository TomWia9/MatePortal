using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class ShopOpinion : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid ShopId { get; set; }

        public Shop Shop { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}