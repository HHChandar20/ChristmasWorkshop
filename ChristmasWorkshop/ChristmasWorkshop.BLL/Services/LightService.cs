using ChristmasWorkshop.BLL.Factories;
using ChristmasWorkshop.BLL.Handlers;
using ChristmasWorkshop.DAL.Data;
using ChristmasWorkshop.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace ChristmasWorkshop.BLL.Services;

public class LightService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly EntityContext _entityContext;
    private readonly ValidationHandler _validationHandler;

    public LightService(IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ApiValidationHandler apiValidationHandler, LocalValidationHandler localValidationHandler)
    {
        _httpContextAccessor = httpContextAccessor;
        _entityContext = entityContext;

        // Set the chain of responsibility
        localValidationHandler.SetNext(apiValidationHandler);
        _validationHandler = localValidationHandler; // Starting handler of the chain
    }

    public Light CreateLight(string description)
    {
        double x, y;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        string christmasToken = httpContext.Request.Headers["Christmas-Token"]!;
        if (string.IsNullOrEmpty(christmasToken))
        {
            throw new UnauthorizedAccessException("Missing Christmas-Token in request headers.");
        }

        do
        {
            x = LightFactory.Random.NextDouble() * 125.8;
            y = 170.3 - (LightFactory.Random.NextDouble() * 155.4);
        } while (!_validationHandler.ValidateAsync(x, y).Result);

        string color = _entityContext.Lights.Any()
            ? LightFactory.GetRandomColor(_entityContext.Lights.ToList().Last().Color)
            : LightFactory.GetRandomColor(null);

        var light = LightFactory.CreateLight(x, y, description, color, LightFactory.GetRandomEffect(), LightFactory.GetRandomRadius(), christmasToken);

        _entityContext.Lights.Add(light);
        _entityContext.SaveChanges();

        return light;
    }

    public List<Light> GetLights()
    {
        return _entityContext.Lights.ToList();
    }
}
