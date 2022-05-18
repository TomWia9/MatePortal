using System;
using MediatR;

namespace Application.ShopOpinions.Commands.UpdateShopOpinion
{
    /// <summary>
    ///     Update shop opinion command
    /// </summary>
    public class UpdateShopOpinionCommand : IRequest
    {
        /// <summary>
        ///     Shop opinion ID
        /// </summary>
        public Guid ShopOpinionId { get; init; }

        /// <summary>
        ///     Shop opinion rate
        /// </summary>
        public int Rate { get; init; }

        /// <summary>
        ///     Shop opinion comment
        /// </summary>
        public string Comment { get; init; }

        /// <summary>
        ///     Shop ID
        /// </summary>
        public Guid ShopId { get; set; }
    }
}