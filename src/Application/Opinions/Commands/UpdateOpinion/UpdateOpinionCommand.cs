using System;
using MediatR;

namespace Application.Opinions.Commands.UpdateOpinion
{
    public class UpdateOpinionCommand : IRequest
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid YerbaMateId { get; set; }
        public Guid UserId { get; set; }
    }
}