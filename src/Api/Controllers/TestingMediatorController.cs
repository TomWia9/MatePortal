using System;
using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
using Application.Brands.Commands.DeleteBrand;
using Application.Brands.Commands.UpdateBrand;
using Application.Brands.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //development purpose only
    [Authorize(Policy = "AdminAccess")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestingMediatorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestingMediatorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("CreateBrand")]
        public async Task<ActionResult<BrandDto>> CreateBrand(CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);

            //return CreatedAtAction("GetBrand", new {id = result.Id}, result);
            return NoContent();
        }

        [HttpPut("UpdateBrand/{id}")]
        public async Task<IActionResult> UpdateBrand(Guid id, UpdateBrandCommand command)
        {
            if (id != command.BrandId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            await _mediator.Send(new DeleteBrandCommand {BrandId = id});

            return NoContent();
        }
    }
}