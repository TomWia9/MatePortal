﻿using System;
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
        /// Yerba mate brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Yerba mate name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Yerba mate description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Yerba mate category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Yerba mate image url
        /// </summary>
        public string imgUrl { get; set; }

        /// <summary>
        /// Yerba ate average price
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// The number of additions of yerba to the favorites
        /// </summary>
        public int NumberOfAddToFav { get; set; }

        /// <summary>
        /// Overrides Mapping method from IMapFrom interface by adding a custom Brand and Category mappings 
        /// </summary>
        /// <param name="profile">Automapper profile</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<YerbaMate, YerbaMateDto>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category.Name));
        }
    }
}