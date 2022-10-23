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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    /// <response code="200">Returns users</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UsersQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetUsersQuery(parameters));
        
        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Gets single user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns>An ActionResult of UserDto</returns>
    /// <response code="200">Returns user</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var result = await Mediator.Send(new GetUserQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Updates the user
    /// </summary>
    /// <param name="command">The update user command</param>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates user</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">User not found</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="403">Forbidden access</response>
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
    /// <returns>An IAction result</returns>
    /// <response code="204">Updates user</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">User not found</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="403">Forbidden access</response>
    [Authorize(Policy = Policies.UserAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(DeleteUserCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}