using System;

namespace Domain.Entities
{
    public class Category
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public YerbaMate YerbaMate { get; set; }
    }
}