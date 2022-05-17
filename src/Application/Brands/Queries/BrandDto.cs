using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Brands.Queries
{
    /// <summary>
    /// Brand data transfer object
    /// </summary>
    public class BrandDto : IMapFrom<Brand>
    {
        /// <summary>
        /// Brand ID
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Brand name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Brand description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Brand country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Overrides Mapping method from IMapFrom interface by adding a custom country mapping
        /// </summary>
        /// <param name="profile">Automapper profile</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Brand, BrandDto>()
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Country.Name));
        }
    }
}