using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Country
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        
        public List<Brand> Brands { get; set; }
    }
}