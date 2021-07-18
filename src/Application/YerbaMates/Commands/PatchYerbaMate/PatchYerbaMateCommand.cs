using System;
using MediatR;

namespace Application.YerbaMates.Commands.PatchYerbaMate
{
    public class PatchYerbaMateCommand : IRequest
    {
        public Guid Id { get; init; }
        public int NumberOfAddToFav { get; set; }
    }
}