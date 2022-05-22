using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers;

//Development purpose only
[Route("api/[controller]")]
public class TestingController : ControllerBase
{
    private readonly ILogger<TestingController> _logger;

    public TestingController(ILogger<TestingController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Debug")]
    public IActionResult LogDebug()
    {
        _logger.LogDebug("Debug");
        return Ok();
    }

    [HttpGet("Info")]
    public IActionResult LogInfo()
    {
        _logger.LogInformation("Info");
        return Ok();
    }

    [HttpGet("Warning")]
    public IActionResult LogWarning()
    {
        _logger.LogWarning("Warning");
        return Ok();
    }

    [HttpGet("Error")]
    public IActionResult LogError()
    {
        _logger.LogError("Error");
        return Ok();
    }

    [HttpGet("Critical")]
    public IActionResult LogCritical()
    {
        _logger.LogCritical("Critical");
        return Ok();
    }
}