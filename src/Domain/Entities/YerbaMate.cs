using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    ///     Yerba mate
    /// </summary>
    public class YerbaMate : BaseEntity, IHasDomainEvent
    {
        /// <summary>
        ///     The yerba mate ID
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     The brand ID
        /// </summary>
        public Guid BrandId { get; set; }

        /// <summary>
        ///     The brand
        /// </summary>
        public Brand Brand { get; set; }

        /// <summary>
        ///     The yerba mate name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The yerba mate description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The yerba mate image url
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        ///     The yerba mate average price
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        ///     The yerba mate category ID
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        ///     The yerba mate category
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        ///     The yerba mate opinions
        /// </summary>
        public IList<Opinion> Opinions { get; set; } = new List<Opinion>();

        /// <summary>
        ///     The yerba mate favourites
        /// </summary>
        public IList<Favourite> Favourites { get; set; } = new List<Favourite>();

        /// <summary>
        ///     Domain events
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}