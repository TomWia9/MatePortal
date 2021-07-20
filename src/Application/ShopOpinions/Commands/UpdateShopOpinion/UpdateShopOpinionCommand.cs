using System;
using MediatR;

namespace Application.ShopOpinions.Commands.UpdateShopOpinion
{
    /// <summary>
    /// Update shop opinion command
    /// </summary>
    public class UpdateShopOpinionCommand : IRequest
    {
        /// <summary>
        /// Shop opinion ID
        /// </summary>
        public Guid ShopOpinionId { get; set; }

        /// <summary>
        /// Shop opinion rate
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Shop opinion comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Shop ID
        /// </summary>
        public Guid ShopId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
    }
}