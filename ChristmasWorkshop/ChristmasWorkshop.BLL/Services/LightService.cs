using ChristmasWorkshop.BLL.Factories;
using ChristmasWorkshop.BLL.Handlers;
using ChristmasWorkshop.DAL.Data;
using ChristmasWorkshop.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace ChristmasWorkshop.BLL.Services;

public class LightService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly EntityContext entityContext;
    private readonly ValidationHandler validationHandler;

    public LightService(IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ApiValidationHandler apiValidationHandler, LocalValidationHandler localValidationHandler)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.entityContext = entityContext;

        // Set the chain of responsibility
        localValidationHandler.SetNext(apiValidationHandler);
        this.validationHandler = localValidationHandler;
    }

    public Light CreateLight(string description)
    {
        double x, y;

        var httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        string christmasToken = httpContext.Request.Headers["Christmas-Token"] !;
        if (string.IsNullOrEmpty(christmasToken))
        {
            throw new UnauthorizedAccessException("Missing Christmas-Token in request headers.");
        }

        do
        {
            x = LightFactory.Random.NextDouble() * 125.8;
            y = 170.3 - (LightFactory.Random.NextDouble() * 155.4);
        }
        while (!this.validationHandler.ValidateAsync(x, y).Result);

        string color = this.entityContext.Lights.Any()
            ? LightFactory.GetRandomColor(this.entityContext.Lights.ToList().Last().Color)
            : LightFactory.GetRandomColor("none");

        var light = LightFactory.CreateLight(x, y, description, color, LightFactory.GetRandomEffect(), LightFactory.GetRandomRadius(), christmasToken);

        this.entityContext.Lights.Add(light);
        this.entityContext.SaveChanges();

        return light;
    }

    public List<Light> GetLights()
    {
        return this.entityContext.Lights.ToList();
    }
}
