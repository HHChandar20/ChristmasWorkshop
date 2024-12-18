using ChristmasWorkshop.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasWorkshop.PL.Controllers;

[ApiController]
[Route("/")]
public class LightController : ControllerBase
{
    private readonly LightService _lightService;

    public LightController(LightService lightService)
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
            return Ok(new { succeeded = true }); // Include succeeded = true
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(500, new { succeeded = false, message = ex.Message });
        }
    }
}