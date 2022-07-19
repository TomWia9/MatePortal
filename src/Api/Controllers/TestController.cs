using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
using Application.Categories.Commands.CreateCategory;
using Application.Users.Queries;
using Application.Users.Queries.GetUsers;
using Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;
using Application.YerbaMates.Commands.CreateYerbaMate;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

//Development purpose only
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize]
[Route("api/[controller]")]
public class TestController : Controller
{
    private readonly IMediator _mediator;
    
    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UsersQueryParameters parameters)
    {
        var result = await _mediator.Send(new GetUsersQuery(parameters));

        return Ok(result);
    }
    
    [HttpPost("createYerbaMateOpinion")]
    public async Task<IActionResult> CreateYerbaMateOpinion(CreateYerbaMateOpinionCommand createYerbaMateOpinion)
    {
        var result = await _mediator.Send(createYerbaMateOpinion);

        return Ok(result);
    }
    
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost("createYerbaMate")]
    public async Task<IActionResult> CreateYerbaMate(CreateYerbaMateCommand createYerbaMate)
    {
        var result = await _mediator.Send(createYerbaMate);

        return Ok(result);
    }
    
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost("createCategory")]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand createCategory)
    {
        var result = await _mediator.Send(createCategory);

        return Ok(result);
    }
    
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost("createBrand")]
    public async Task<IActionResult> CreateBrand(CreateBrandCommand createBrand)
    {
        var result = await _mediator.Send(createBrand);

        return Ok(result);
    }
}