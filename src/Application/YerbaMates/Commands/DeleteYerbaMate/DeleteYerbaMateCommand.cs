using System;
using MediatR;

namespace Application.YerbaMates.Commands.DeleteYerbaMate
{
    public class DeleteYerbaMateCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}