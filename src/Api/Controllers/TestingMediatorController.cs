using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
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
    }
}