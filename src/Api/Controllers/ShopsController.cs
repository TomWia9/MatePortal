using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Shops.Queries;
using Application.Shops.Commands.CreateShop;
using Application.Shops.Commands.DeleteShop;
using Application.Shops.Commands.UpdateShop;
using Application.Shops.Queries.GetShop;
using Application.Shops.Queries.GetShops;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Shops controller
/// </summary>
public class ShopsController : ApiControllerBase
{
    /// <summary>
    ///     Gets shop by id
    /// </summary>
    /// <returns>An ActionResult of type ShopDto</returns>
    /// <response code="200">Returns shop</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShopDto>> GetShop(Guid id)
    {
        var result = await Mediator.Send(new GetShopQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Gets shops
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of ShopDto</returns>
    /// <response code="200">Returns shops</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ShopDto>>> GetShops(
        [FromQuery] ShopsQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetShopsQuery(parameters));

        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Creates shop
    /// </summary>
    /// <returns>An ActionResult of type ShopDto</returns>
    /// <response code="201">Creates shop</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Shop conflicts with another shop</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost]
    public async Task<ActionResult<ShopDto>> CreateShop(CreateShopCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetShop", new {id = result.Id}, result);
    }

    /// <summary>
    ///     Update shop
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates shop</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Shop not found</response>
    /// <response code="409">Shop conflicts with another shop</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateShop(UpdateShopCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Deletes shop
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes shop</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Shop not found</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteShop(DeleteShopCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}