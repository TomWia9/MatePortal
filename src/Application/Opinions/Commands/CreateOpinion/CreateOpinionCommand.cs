using System;
using Application.Opinions.Queries;
using MediatR;

namespace Application.Opinions.Commands.CreateOpinion
{
    /// <summary>
    /// Create opinion command
    /// </summary>
    public class CreateOpinionCommand : IRequest<OpinionDto>
    {
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
    }
}