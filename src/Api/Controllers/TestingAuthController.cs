using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //Development purpose only
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class TestingAuthController : ControllerBase
    {
        private readonly IHttpService _httpService;

        public TestingAuthController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [Authorize(Roles = "User")]
        [HttpGet("getUser")]
        public IActionResult GetUserId()
        {
            var userId = _httpService.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            return Ok(new {Id = userId, Role = HttpContext.User.IsInRole(Roles.User) ? "User" : "Not user"});
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getAdmin")]
        public IActionResult GetAdminId()
        {
            var userId = _httpService.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            return Ok(new
            {
                Id = userId,
                Role = HttpContext.User.IsInRole(Roles.Administrator) ? "Administrator" : "Not Administrator"
            });
        }
    }
}