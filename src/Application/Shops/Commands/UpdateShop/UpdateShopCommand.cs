using System;
using MediatR;

namespace Application.Shops.Commands.UpdateShop
{
    /// <summary>
    ///     Update shop command
    /// </summary>
    public class UpdateShopCommand : IRequest
    {
        /// <summary>
        ///     Shop ID
        /// </summary>
        public Guid ShopId { get; set; }

        /// <summary>
        ///     Shop name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Shop description
        /// </summary>
        public string Description { get; set; }
    }
}