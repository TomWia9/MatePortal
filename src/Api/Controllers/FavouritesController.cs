using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Favourites.Commands.CreateFavourite;
using Application.Favourites.Commands.DeleteFavourite;
using Application.Favourites.Queries;
using Application.Favourites.Queries.GetFavourites;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Favourites controller
/// </summary>
public class FavouritesController : ApiControllerBase
{
    /// <summary>
    ///     Gets user's favourites yerba mates ids
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of FavouriteDto</returns>
    /// <response code="200">Returns user favourites yerba mates ids</response>
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<PaginatedList<FavouriteDto>>> GetFavourites(Guid userId,
        [FromQuery] FavouritesQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetFavouritesQuery(userId, parameters));

        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Creates favourite yerba mate
    /// </summary>
    /// <returns>An ActionResult of type FavouriteDto</returns>
    /// <response code="201">Creates favourite yerba mate</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Favourite yerba mate conflicts with another favourite yerba mate</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpPost]
    public async Task<ActionResult<FavouriteDto>> CreateFavourite(CreateFavouriteCommand command)
    {
        var result = await Mediator.Send(command);

        return Created("GetFavourites", result);
    }

    /// <summary>
    ///     Deletes favourite yerba mate
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes favourite yerba mate</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Favourite yerba mate not found</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteFavourite(DeleteFavouriteCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}