using System;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //development purpose only
    [Route("api/[controller]")]
    public class TestingExceptionsController : ControllerBase
    {
        [HttpGet("forbidden")]
        public IActionResult ForbiddenEx()
        {
            throw new ForbiddenAccessException();
        }

        [HttpGet("unauthorized")]
        public IActionResult UnauthorizedEx()
        {
            throw new UnauthorizedAccessException("test unauthorized exception");
        }

        [HttpGet("notfound")]
        public IActionResult NotFoundEx()
        {
            throw new NotFoundException("test notFound exception");
        }

        [HttpGet("validation")]
        public IActionResult ValidationEx()
        {
            throw new ValidationException();
        }
    }
}