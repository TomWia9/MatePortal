using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    ///     The shop
    /// </summary>
    public class Shop : BaseEntity, IHasDomainEvent
    {
        /// <summary>
        ///     The shop ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     The shop name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The shop description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The shop opinions
        /// </summary>
        public IList<ShopOpinion> Opinions { get; set; }

        /// <summary>
        ///     Domain events
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}