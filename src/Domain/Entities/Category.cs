using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// The category
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// The category ID
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// The category name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The category description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The category yerba mates
        /// </summary>
        public IList<YerbaMate> YerbaMate { get; set; } = new List<YerbaMate>();

        /// <summary>
        /// Domain events
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}