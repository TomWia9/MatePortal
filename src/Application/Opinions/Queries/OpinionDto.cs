using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Opinions.Queries
{
    /// <summary>
    /// Opinion data transfer object
    /// </summary>
    public class OpinionDto : IMapFrom<Opinion>
    {
        /// <summary>
        /// Opinion ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Opinion rate
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Opinion comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Yerba mate ID 
        /// </summary>
        public Guid YerbaMateId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Overrides Mapping method from IMapFrom interface by adding a custom UserId and YerbaMateId mappings
        /// </summary>
        /// <param name="profile">Automapper profile</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Opinion, OpinionDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.YerbaMateId, opt => opt.MapFrom(s => s.YerbaMate.Id));
        }
    }
}