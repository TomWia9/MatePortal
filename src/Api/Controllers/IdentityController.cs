using System.Threading.Tasks;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
}