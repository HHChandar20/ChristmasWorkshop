using ChristmasWorkshop.DAL.Data;
using ChristmasWorkshop.DAL.Models;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace ChristmasWorkshop.BLL.Services;

public class LightService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static EntityContext _entityContext;
    private static readonly Random Random = new Random();
    private static readonly string[] Colors = { "blue-lt", "blue-dk", "red", "gold-lt", "gold-dk"};
    private static readonly string[] Effects = { "g1", "g2", "g3"};

    public LightService(IHttpContextAccessor httpContextAccessor, EntityContext entityContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _entityContext = entityContext;
    }

    public Light CreateLight(string description)
    {
        double x, y;
        
        var httpContext = _httpContextAccessor.HttpContext;
        
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }
        
        // Retrieve Christmas-Token from the request headers
        string christmasToken = httpContext.Request.Headers["Christmas-Token"]!;
        
        if (string.IsNullOrEmpty(christmasToken))
        {
            throw new UnauthorizedAccessException("Missing Christmas-Token in request headers.");
        }

        do
        {
            x = Random.NextDouble() * 125.8;
            y = 170.3 - (Random.NextDouble() * 155.4);
        } while (!IsValidPoint(x, y).Result);
        
        Light light = new Light
        {
            X = x,
            Y = y,
            Radius = Random.Next(3, 7),
            Color = Colors[Random.Next(Colors.Length)],
            Effect = Effects[Random.Next(Effects.Length)],
            Description = description,
            ChristmasToken = "abcdefg"
        };
        
        _entityContext.Lights.Add(light);
        _entityContext.SaveChanges();

        return light;
    }
    
    private async Task<bool> IsValidPoint(double x, double y)
    {
        bool localCheck = (170.3 - y) >= (1.357 * Math.Abs(x - 62.9));

        if (!localCheck)
        {
            return false;
        }

        string url = $"https://polygon.gsk567.com/?x={x}&y={y}";
        var client = new RestClient(url);
        var request = new RestRequest()
            .AddQueryParameter("x", x.ToString())
            .AddQueryParameter("y", y.ToString());
        var response = await client.ExecuteAsync(request);
        
        if (response.IsSuccessful)
        {
            Console.WriteLine(response.Content.Contains("true"));
            return response?.Content?.Contains("true") ?? false;
        }
        else
        {
            throw new Exception($"Error calling polygon API: {response.StatusCode}");
        }
    }

    public List<Light> GetLights()
    {
        return _entityContext.Lights.ToList();
    }
    
    public void DeleteLightsByToken(string token)
    {
        var lightsToDelete = _entityContext.Lights.Where(l => l.ChristmasToken != token).ToList();
        _entityContext.Lights.RemoveRange(lightsToDelete);
        _entityContext.SaveChanges();
    }

}