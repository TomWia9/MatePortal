using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.ShopOpinions.Commands.CreateShopOpinion;
using Application.ShopOpinions.Commands.DeleteShopOpinion;
using Application.ShopOpinions.Commands.UpdateShopOpinion;
using Application.ShopOpinions.Queries;
using Application.ShopOpinions.Queries.GetShopOpinion;
using Application.ShopOpinions.Queries.GetShopOpinions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Shop opinions controller
/// </summary>
public class ShopOpinionsController : ApiControllerBase
{
    /// <summary>
    ///     Gets shop opinion by id
    /// </summary>
    /// <returns>An ActionResult of type ShopOpinionDto</returns>
    /// <response code="200">Returns shop opinion</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShopOpinionDto>> GetShopOpinion(Guid id)
    {
        var result = await Mediator.Send(new GetShopOpinionQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Gets shop opinions
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of ShopOpinionDto</returns>
    /// <response code="200">Returns shop opinions</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ShopOpinionDto>>> GetShopOpinions(
        [FromQuery] ShopOpinionsQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetShopOpinionsQuery(parameters));

        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Creates shop opinion
    /// </summary>
    /// <returns>An ActionResult of type ShopOpinionDto</returns>
    /// <response code="201">Creates shop opinion</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Shop opinion conflicts with another shop opinion</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpPost]
    public async Task<ActionResult<ShopOpinionDto>> CreateShopOpinion(CreateShopOpinionCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetShopOpinion", new {id = result.Id}, result);
    }

    /// <summary>
    ///     Update shop opinion
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates shop opinion</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Shop opinion not found</response>
    /// <response code="409">Shop opinion conflicts with another shop opinion</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateShopOpinion(UpdateShopOpinionCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Deletes shop opinion
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes shop opinion</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Shop opinion not found</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteShopOpinion(DeleteShopOpinionCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}