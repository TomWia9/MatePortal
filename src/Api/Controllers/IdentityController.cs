using System.Threading.Tasks;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     The identity controller
/// </summary>
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    /// <summary>
    ///     The mediator
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    ///     Initializes IdentityController
    /// </summary>
    /// <param name="mediator">The mediator</param>
    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Registers the user
    /// </summary>
    /// <param name="request">The register request</param>
    /// <returns>An IActionResult</returns>
    /// <response code="200">Creates user and returns jwt token</response>
    /// <response code="400">Creates user and returns jwt token</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        var result = await _mediator.Send(request);

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
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
    {
        var result = await _mediator.Send(request);

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