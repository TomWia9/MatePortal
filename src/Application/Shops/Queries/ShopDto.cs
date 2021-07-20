using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Shops.Queries
{
    /// <summary>
    /// Shop data transfer object
    /// </summary>
    public class ShopDto : IMapFrom<Shop>
    {
        /// <summary>
        /// Shop ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Shop name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Shop description
        /// </summary>
        public string Description { get; set; }
    }
}