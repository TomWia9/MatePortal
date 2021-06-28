using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Opinion : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        
        public YerbaMate YerbaMate { get; set; }
        public Guid UserId { get; set; }
        
        public List<DomainEvent> DomainEvents { get; set; }
    }
}