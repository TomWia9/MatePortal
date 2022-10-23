using System;
using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
using Application.Brands.Commands.DeleteBrand;
using Application.Brands.Commands.UpdateBrand;
using Application.Brands.Queries;
using Application.Brands.Queries.GetBrand;
using Application.Brands.Queries.GetBrands;
using Application.Common.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

/// <summary>
///     Brands controller
/// </summary>
public class BrandsController : ApiControllerBase
{
    /// <summary>
    ///     Gets brand by id
    /// </summary>
    /// <returns>An ActionResult of type BrandDto</returns>
    /// <response code="200">Returns brand</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BrandDto>> GetBrand(Guid id)
    {
        var result = await Mediator.Send(new GetBrandQuery(id));

        return Ok(result);
    }

    /// <summary>
    ///     Gets brands
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of BrandDto</returns>
    /// <response code="200">Returns brands</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<BrandDto>>> GetBrands([FromQuery] BrandsQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetBrandsQuery(parameters));
        
        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }

    /// <summary>
    ///     Creates brand
    /// </summary>
    /// <returns>An ActionResult of type BrandDto</returns>
    /// <response code="201">Creates brand</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Brand conflicts with another brand</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost]
    public async Task<ActionResult<BrandDto>> CreateBrand(CreateBrandCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetBrand", new {id = result.Id}, result);
    }

    /// <summary>
    ///     Update brand
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates brand</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Brand not found</response>
    /// <response code="409">Brand conflicts with another brand</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateBrand(UpdateBrandCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    ///     Deletes brand
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes brand</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Brand not found</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteBrand(DeleteBrandCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}