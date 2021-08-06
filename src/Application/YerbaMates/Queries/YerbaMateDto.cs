using System;
using Application.Brands.Queries;
using Application.Categories.Queries;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.YerbaMates.Queries
{
    /// <summary>
    /// Yerba mate data transfer object
    /// </summary>
    public class YerbaMateDto : IMapFrom<YerbaMate>
    {
        /// <summary>
        /// Yerba mate ID
        /// </summary>
        public Guid Id { get; init; }


        /// <summary>
        /// Yerba mate name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Yerba mate description
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Yerba mate image url
        /// </summary>
        public string imgUrl { get; set; }

        /// <summary>
        /// Yerba ate average price
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// Yerba mate brand
        /// </summary>
        public BrandDto Brand { get; set; }

        /// <summary>
        /// Yerba mate category
        /// </summary>
        public CategoryDto Category { get; set; }
    }
}