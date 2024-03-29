﻿using Application.Shops.Queries;
using MediatR;

namespace Application.Shops.Commands.CreateShop;

/// <summary>
///     Create shop command
/// </summary>
public class CreateShopCommand : IRequest<ShopDto>
{
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