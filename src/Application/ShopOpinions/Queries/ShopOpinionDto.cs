using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.ShopOpinions.Queries
{
    /// <summary>
    /// Shop opinion data transfer object
    /// </summary>
    public class ShopOpinionDto : IMapFrom<ShopOpinion>
    {
        /// <summary>
        /// Shop opinion ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Shop opinion rate
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Shop opinion comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Shop opinion created date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Shop ID
        /// </summary>
        public Guid ShopId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public Guid CreatedBy { get; set; }
    }
}