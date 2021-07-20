using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.ShopOpinions.Queries
{
    /// <summary>
    /// Shop opinion data transfer object
    /// </summary>
    public class ShopOpinionDto : IMapFrom<ShopOpinion>
    {
        /// <summary>
        /// Shop opinion ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Shop opinion rate
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Shop opinion comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Shop ID
        /// </summary>
        public Guid ShopId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Overrides Mapping method from IMapFrom interface by adding a custom ShopId and UserId mappings 
        /// </summary>
        /// <param name="profile">Automapper profile</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ShopOpinion, ShopOpinionDto>()
                .ForMember(d => d.ShopId, opt => opt.MapFrom(s => s.Shop.Id))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.CreatedBy));
        }
    }
}