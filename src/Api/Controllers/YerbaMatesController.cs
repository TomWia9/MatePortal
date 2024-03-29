﻿using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.YerbaMateImages.Commands.CreateYerbaMateImage;
using Application.YerbaMateImages.Commands.DeleteYerbaMateImage;
using Application.YerbaMates.Commands.CreateYerbaMate;
using Application.YerbaMates.Commands.DeleteYerbaMate;
using Application.YerbaMates.Commands.UpdateYerbaMate;
using Application.YerbaMates.Queries;
using Application.YerbaMates.Queries.GetYerbaMate;
using Application.YerbaMates.Queries.GetYerbaMates;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Yerba Mates controller
/// </summary>
public class YerbaMatesController : ApiControllerBase
{
    /// <summary>
    ///     Gets yerba mate by id
    /// </summary>
    /// <returns>An ActionResult of type YerbaMateDto</returns>
    /// <response code="200">Returns yerba mate</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<YerbaMateDto>> GetYerbaMate(Guid id)
    {
        var result = await Mediator.Send(new GetYerbaMateQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Gets yerba mates
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of YerbaMateDto</returns>
    /// <response code="200">Returns yerba mates</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<YerbaMateDto>>> GetYerbaMates(
        [FromQuery] YerbaMatesQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetYerbaMatesQuery(parameters));

        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Creates yerba mate
    /// </summary>
    /// <returns>An ActionResult of type YerbaMateDto</returns>
    /// <response code="201">Creates yerba mate</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Yerba Mate conflicts with another yerba mate</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost]
    public async Task<ActionResult<YerbaMateDto>> CreateYerbaMate(CreateYerbaMateCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetYerbaMate", new {id = result.Id}, result);
    }

    /// <summary>
    ///     Update yerba mate
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates yerba mate</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Yerba mate not found</response>
    /// <response code="409">Yerba mate conflicts with another yerba mate</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateYerbaMate(UpdateYerbaMateCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Deletes yerba mate
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes yerba mate</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Yerba mate not found</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteYerbaMate(DeleteYerbaMateCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Creates yerba mate image
    /// </summary>
    /// <returns>An ActionResult of type YerbaMateDto</returns>
    /// <response code="201">Creates yerba mate image</response>
    /// <response code="400">Bad request</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost("createImage")]
    public async Task<ActionResult<YerbaMateDto>> CreateYerbaMateImage(CreateYerbaMateImageCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetYerbaMate", new {id = result.YerbaMateId}, result);
    }

    /// <summary>
    ///     Deletes yerba mate image
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes yerba mate images</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Yerba mate image not found</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpDelete("deleteImage")]
    public async Task<IActionResult> DeleteYerbaMateImage(DeleteYerbaMateImageCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}