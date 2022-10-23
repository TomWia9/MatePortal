using System.Threading.Tasks;
using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries;
using Application.Categories.Queries.GetCategories;
using Application.Common.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

/// <summary>
///     Categories controller
/// </summary>
public class CategoriesController : ApiControllerBase
{
    /// <summary>
    ///     Gets categories
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of CategoryDto</returns>
    /// <response code="200">Returns categories</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CategoryDto>>> GetCategories([FromQuery] CategoriesQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetCategoriesQuery(parameters));
        
        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }
    
    /// <summary>
    ///     Creates category
    /// </summary>
    /// <returns>An ActionResult of type CategoryDto</returns>
    /// <response code="201">Creates category</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Category conflicts with another category</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetCategories", result);
    }
    
    /// <summary>
    ///     Update category
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates category</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Category not found</response>
    /// <response code="409">Category conflicts with another category</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
    
    /// <summary>
    ///     Deletes category
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes category</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Category not found</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(DeleteCategoryCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}