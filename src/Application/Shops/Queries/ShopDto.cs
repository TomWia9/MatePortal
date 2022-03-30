using System;
using Application.Common.Mappings;
using AutoMapper;
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

        /// <summary>
        /// Number of shop opinions
        /// </summary>
        public int NumberOfShopOpinions { get; set; }

        /// <summary>
        /// Overrides Mapping method from IMapFrom interface by adding a custom NumberOfShopOpinions mapping
        /// </summary>
        /// <param name="profile">Automapper profile</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Shop, ShopDto>()
                .ForMember(d => d.NumberOfShopOpinions, opt => opt.MapFrom(s => s.Opinions.Count));
        }
    }
}