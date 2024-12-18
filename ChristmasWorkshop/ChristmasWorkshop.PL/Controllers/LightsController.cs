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


    public class DescriptionData
    {
        public string? Desc { get; set; }
    }

    [HttpPost]
    public IActionResult CreateLight([FromBody] DescriptionData Desc)
    {
        if (string.IsNullOrEmpty(Desc.Desc))
        {
            return BadRequest(new { succeeded = false, message = "Description is required." });
        }

        try
        {
            var light = _lightService.CreateLight(Desc.Desc);
            return Ok(new { succeeded = true, light = light }); // Include succeeded = true
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