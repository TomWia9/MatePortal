using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.GetUsers;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Users controller
/// </summary>
public class UsersController : ApiControllerBase
{
    /// <summary>
    ///     Gets users
    /// </summary>
    /// <param name="parameters">Query parameters to apply</param>
    /// <returns>An ActionResult of type IEnumerable of UserDto</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UsersQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetUsersQuery(parameters));

        return Ok(result);
    }

    /// <summary>
    ///     Gets single user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns>An ActionResult of UserDto</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var result = await Mediator.Send(new GetUserQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Updates the user
    /// </summary>
    /// <param name="command">The udate user command</param>
    /// <returns>An IActionResult</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Policies.UserAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Deletes the user
    /// </summary>
    /// <param name="command">The delete user command</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = Policies.UserAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(DeleteUserCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}