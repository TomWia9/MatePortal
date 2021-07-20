using System;
using Application.Opinions.Queries;
using MediatR;

namespace Application.Opinions.Commands.CreateOpinion
{
    public class CreateOpinionCommand : IRequest<OpinionDto>
    {
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid YerbaMateId { get; set; }
        public Guid UserId { get; set; }
    }
}