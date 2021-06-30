using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Favourites.Queries.GetFavourites
{
    public class FavouriteDto : IMapFrom<Favourite>
    {
        public Guid Id { get; set; }
        public Guid YerbaMateId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Favourite, FavouriteDto>()
                .ForMember(d => d.YerbaMateId, opt => opt.MapFrom(s => s.YerbaMate.Id));
        }
    }
}