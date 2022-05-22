using System;
using MediatR;

namespace Application.Shops.Commands.DeleteShop;

/// <summary>
///     Delete shop command
/// </summary>
public class DeleteShopCommand : IRequest
{
    /// <summary>
    ///     Shop ID
    /// </summary>
    public Guid ShopId { get; init; }
}