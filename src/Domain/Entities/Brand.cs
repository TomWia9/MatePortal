using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Brand : BaseEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
       
        public Country Country { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}