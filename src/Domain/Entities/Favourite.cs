using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Favourite : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }
        public YerbaMate YerbaMate { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}