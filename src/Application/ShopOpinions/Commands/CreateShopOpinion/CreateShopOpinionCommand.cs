using System;
using Application.ShopOpinions.Queries;
using MediatR;

namespace Application.ShopOpinions.Commands.CreateShopOpinion
{
    /// <summary>
    /// Create shop opinion command
    /// </summary>
    public class CreateShopOpinionCommand : IRequest<ShopOpinionDto>
    {
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
    }
}