using System.Threading.Tasks;
using Application.Countries.Queries;
using Application.Countries.Queries.GetCountries;
using Application.Common.Models;
using Application.Countries.Commands.CreateCountry;
using Application.Countries.Commands.DeleteCountry;
using Application.Countries.Commands.UpdateCountry;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

public class CountriesController : ApiControllerBase
{
    /// <summary>
    ///     Gets countries
    /// </summary>
    /// <returns>An ActionResult of type IEnumerable of CountryDto</returns>
    /// <response code="200">Returns countries</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CountryDto>>> GetCountries([FromQuery] CountriesQueryParameters parameters)
    {
        var result = await Mediator.Send(new GetCountriesQuery(parameters));
        
        Response.Headers.Add("X-Pagination", result.GetMetadata());

        return Ok(result);
    }
    
    /// <summary>
    ///     Creates country
    /// </summary>
    /// <returns>An ActionResult of type CountryDto</returns>
    /// <response code="201">Creates country</response>
    /// <response code="400">Bad request</response>
    /// <response code="409">Country conflicts with another country</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPost]
    public async Task<ActionResult<CountryDto>> CreateCountry(CreateCountryCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction("GetCountries", result);
    }
    
    /// <summary>
    ///     Update country
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Updates country</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Country not found</response>
    /// <response code="409">Country conflicts with another country</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpPut]
    public async Task<IActionResult> UpdateCountry(UpdateCountryCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
    
    /// <summary>
    ///     Deletes country
    /// </summary>
    /// <returns>An IActionResult</returns>
    /// <response code="204">Deletes country</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Country not found</response>
    [Authorize(Policy = Policies.AdminAccess)]
    [HttpDelete]
    public async Task<IActionResult> DeleteCountry(DeleteCountryCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}