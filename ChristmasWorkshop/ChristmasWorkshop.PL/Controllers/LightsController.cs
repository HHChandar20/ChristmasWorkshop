using ChristmasWorkshop.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasWorkshop.PL.Controllers;

[ApiController]
[Route("/")]
public class LightsController : ControllerBase
{
    private readonly LightService _lightService;

    public LightsController(LightService lightService)
    {
        _lightService = lightService;
    }

    [HttpGet]
    public IActionResult GetLights()
    {
        return Ok(_lightService.GetLights());
    }
    
    
    [HttpPost]
    public IActionResult CreateLight([FromBody] string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            return BadRequest("Description is required.");
        }

        try
        {
            var light = _lightService.CreateLight(description);
            return Ok(light);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { succeeded = false, message = ex.Message });
        }
    }
    
    [HttpDelete("expired")]
    public IActionResult DeleteLightsByToken([FromQuery] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Christmas token is required.");
        }

        _lightService.DeleteLightsByToken(token);
        return Ok(new { message = "Expired lights deleted successfully." });
    }
}