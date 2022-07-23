using System.Threading.Tasks;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     The identity controller
/// </summary>
[Route("api/[controller]")]
public class IdentityController : ApiControllerBase
{
    /// <summary>
    ///     Registers the user
    /// </summary>
    /// <param name="request">The register request</param>
    /// <returns>An IActionResult</returns>
    /// <response code="200">Creates user and returns jwt token</response>
    /// <response code="400">Bad request</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        var result = await Mediator.Send(request);

        if (!result.Success)
            return BadRequest(new AuthFailedResponse
            {
                ErrorMessages = result.ErrorMessages
            });

        return Ok(new AuthSuccessResponse
        {
            Token = result.Token
        });
    }

    /// <summary>
    ///     Authenticates the user
    /// </summary>
    /// <param name="request">The user login request</param>
    /// <returns>An IActionResult</returns>
    /// <response code="200">Authenticates user and returns jwt token</response>
    /// <response code="400">Bad request</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
    {
        var result = await Mediator.Send(request);

        if (!result.Success)
            return BadRequest(new AuthFailedResponse
            {
                ErrorMessages = result.ErrorMessages
            });

        return Ok(new AuthSuccessResponse
        {
            Token = result.Token
        });
    }
}