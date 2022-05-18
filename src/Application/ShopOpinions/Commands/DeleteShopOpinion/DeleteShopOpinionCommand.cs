using System;
using MediatR;

namespace Application.ShopOpinions.Commands.DeleteShopOpinion
{
    /// <summary>
    ///     Delete shop opinion command
    /// </summary>
    public class DeleteShopOpinionCommand : IRequest
    {
        /// <summary>
        ///     Shop opinion ID
        /// </summary>
        public Guid ShopOpinionId { get; init; }
    }
}