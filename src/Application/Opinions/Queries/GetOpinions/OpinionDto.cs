using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Opinions.Queries.GetOpinions
{
    public class OpinionDto : IMapFrom<Opinion>
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid YerbaMateId { get; set; }
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Opinion, OpinionDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.YerbaMateId, opt => opt.MapFrom(s => s.YerbaMate.Id));
        }
    }
}