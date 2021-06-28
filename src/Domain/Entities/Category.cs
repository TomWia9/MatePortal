using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntity, IHasDomainEvent 
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public YerbaMate YerbaMate { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}