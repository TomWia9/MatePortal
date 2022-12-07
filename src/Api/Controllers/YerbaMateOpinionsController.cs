using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;
using Application.YerbaMateOpinions.Commands.DeleteYerbaMateOpinion;
using Application.YerbaMateOpinions.Commands.UpdateYerbaMateOpinion;
using Application.YerbaMateOpinions.Queries;
using Application.YerbaMateOpinions.Queries.GetYerbaMateOpinion;
using Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Yerba mate opinions controller
/// </summary>
public class YerbaMateOpinionsController : ApiControllerBase
{
    /// <summary>
    ///     Gets yerba mate opinion by id
    /// </summary>
    /// <returns>An ActionResult of type YerbaMateOpinionDto</returns>
    /// <response code="200">Returns yerba mate opinion</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<YerbaMateOpinionDto>> GetYerbaMateOpinion(Guid id)
    {
        var result = await Mediator.Send(new GetYerbaMateOpinionQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Gets yerba mate opinions
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of YerbaMateOpinionDto</returns>
    /// <response code="200">Returns yerba mate opinions</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<YerbaMateOpinionDto>>> GetYerbaMateOpinions(
        [FromQuery] YerbaMateOpinionsQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));

        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Creates yerba mate opinion
    /// </summary>
    /// <returns>An ActionResult of type YerbaMateOpinionDto</returns>
    /// <response code="201">Creates yerba mate opinion</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Yerba Mate opinion conflicts with another yerba mate opinion</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpPost]
    public async Task<ActionResult<YerbaMateOpinionDto>> CreateYerbaMateOpinion(CreateYerbaMateOpinionCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetYerbaMateOpinion", new {id = result.Id}, result);
    }

    /// <summary>
    ///     Update yerba mate opinion
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates yerba mate opinion</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Yerba mate opinion not found</response>
    /// <response code="409">Yerba mate opinion conflicts with another yerba mate opinion</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateYerbaMateOpinion(UpdateYerbaMateOpinionCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Deletes yerba mate opinion
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes yerba mate opinion</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Yerba mate opinion not found</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteYerbaMateOpinion(DeleteYerbaMateOpinionCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}