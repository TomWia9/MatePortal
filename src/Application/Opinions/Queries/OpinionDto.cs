using System;
using Application.Common.Mappings;
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
        /// Opinion created date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public Guid CreatedBy { get; set; }
    }
}