using System;
using MediatR;

namespace Application.YerbaMates.Commands.PatchYerbaMate
{
    /// <summary>
    /// Partially update yerba mate command
    /// </summary>
    public class PatchYerbaMateCommand : IRequest
    {
        /// <summary>
        /// Yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; init; }

        /// <summary>
        /// The number of additions of yerba to the favorites
        /// </summary>
        public int NumberOfAddToFav { get; set; }
    }
}