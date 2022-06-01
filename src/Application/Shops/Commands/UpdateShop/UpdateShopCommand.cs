using System;
using MediatR;

namespace Application.Shops.Commands.UpdateShop;

/// <summary>
///     Update shop command
/// </summary>
public class UpdateShopCommand : IRequest
{
    /// <summary>
    ///     Shop ID
    /// </summary>
    public Guid ShopId { get; init; }

    /// <summary>
    ///     Shop name
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Shop description
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    ///     Shop url
    /// </summary>
    public string Url { get; init; }
}