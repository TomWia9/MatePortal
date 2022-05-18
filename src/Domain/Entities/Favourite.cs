using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    ///     The favourite
    /// </summary>
    public class Favourite : BaseEntity, IHasDomainEvent
    {
        /// <summary>
        ///     The favourite ID
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     The yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; set; }

        /// <summary>
        ///     The yerba mate
        /// </summary>
        public YerbaMate YerbaMate { get; set; }

        /// <summary>
        ///     Domain events
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}