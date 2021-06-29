﻿using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntity 
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IList<YerbaMate> YerbaMate { get; set; } = new List<YerbaMate>();

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}