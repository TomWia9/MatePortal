using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    ///     The brand
    /// </summary>
    public class Brand : BaseEntity
    {
        /// <summary>
        ///     The brand ID
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     The brand name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The brand description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The brand country ID
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        ///     The brand country
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     The brand yerba mates
        /// </summary>
        public IList<YerbaMate> YerbaMate { get; set; } = new List<YerbaMate>();

        /// <summary>
        ///     Domain events
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}