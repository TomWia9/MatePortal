using System;
using MediatR;

namespace Application.Opinions.Commands.DeleteOpinion
{
    public class DeleteOpinionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}