using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.YerbaMates.Queries
{
    public class YerbaMateDto : IMapFrom<YerbaMate>
    {
        public Guid Id { get; init; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string imgUrl { get; set; }
        public decimal AveragePrice { get; set; }
        public int NumberOfAddToFav { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<YerbaMate, YerbaMateDto>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category.Name));
        }
    }
}