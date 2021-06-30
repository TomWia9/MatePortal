using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Brands.Queries.GetBrands
{
    public class BrandDto : IMapFrom<Brand>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Brand, BrandDto>()
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Country.Name));
        }
    }
}