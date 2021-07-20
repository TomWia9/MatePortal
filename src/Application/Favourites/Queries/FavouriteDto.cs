using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Favourites.Queries
{
    /// <summary>
    /// Favourite data transfer object
    /// </summary>
    public class FavouriteDto : IMapFrom<Favourite>
    {
        /// <summary>
        /// Favourite ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; set; }

        /// <summary>
        /// Overrides Mapping method from IMapFrom interface by adding a custom YerbaMateId mapping 
        /// </summary>
        /// <param name="profile">Automapper profile</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Favourite, FavouriteDto>()
                .ForMember(d => d.YerbaMateId, opt => opt.MapFrom(s => s.YerbaMate.Id));
        }
    }
}