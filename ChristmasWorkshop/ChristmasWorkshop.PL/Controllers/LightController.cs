using ChristmasWorkshop.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasWorkshop.PL.Controllers;

[ApiController]
[Route("/")]
public class LightController : ControllerBase
{
    private readonly LightService lightService;

    public LightController(LightService lightService)
    {
        this.lightService = lightService;
    }

    [HttpGet]
    public IActionResult GetLights()
    {
        return this.Ok(this.lightService.GetLights());
    }

    public class DescriptionData
    {
        public string? Desc { get; set; }
    }

    [HttpPost]
    public IActionResult CreateLight([FromBody] DescriptionData desc)
    {
        if (string.IsNullOrEmpty(desc.Desc))
        {
            return this.BadRequest(new { succeeded = false, message = "Description is required." });
        }

        try
        {
            var light = this.lightService.CreateLight(desc.Desc);
            return this.Ok(new { succeeded = true }); // Include succeeded = true
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return this.StatusCode(500, new { succeeded = false, message = ex.Message });
        }
    }
}