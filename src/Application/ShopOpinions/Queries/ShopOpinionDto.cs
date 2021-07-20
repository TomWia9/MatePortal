using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.ShopOpinions.Queries
{
    public class ShopOpinionDto : IMapFrom<ShopOpinion>
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ShopOpinion, ShopOpinionDto>()
                .ForMember(d => d.ShopId, opt => opt.MapFrom(s => s.Shop.Id))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.CreatedBy));
        }
    }
}