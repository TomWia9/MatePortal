using System;
using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
using Application.Brands.Commands.DeleteBrand;
using Application.Brands.Commands.UpdateBrand;
using Application.Brands.Queries;
using Application.Brands.Queries.GetBrand;
using Application.Brands.Queries.GetBrands;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Controllers
{
    //development purpose only
    [Route("api/[controller]")]
    [ApiController]
    public class TestingMediatorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestingMediatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetBrand/{id}")]
        public async Task<ActionResult<BrandDto>> GetBrand(Guid id)
        {
            return await _mediator.Send(new GetBrandQuery(id));
        }
        
        [HttpGet("GetBrands")]
        public async Task<ActionResult<PaginatedList<BrandDto>>> GetBrands([FromQuery] BrandsQueryParameters parameters)
        {
            var result = await _mediator.Send(new GetBrandsQuery(parameters));
            
            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));

            return Ok(result);
        }

        [Authorize(Policy = "AdminAccess")]
        [HttpPost("CreateBrand")]
        public async Task<ActionResult<BrandDto>> CreateBrand(CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction("GetBrand", new {id = result.Id}, result);
        }

        [Authorize(Policy = "AdminAccess")]
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

        [Authorize(Policy = "AdminAccess")]
        [HttpDelete("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            await _mediator.Send(new DeleteBrandCommand {BrandId = id});

            return NoContent();
        }
    }
}