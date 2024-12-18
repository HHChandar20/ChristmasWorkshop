using ChristmasWorkshop.DAL.Data;
using Microsoft.AspNetCore.Http;

public class TokenService
{
    private readonly EntityContext _entityContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(EntityContext entityContext, IHttpContextAccessor httpContextAccessor)
    {
        _entityContext = entityContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        string token = httpContext.Request.Headers["Christmas-Token"]!;
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("Missing Christmas-Token in request headers.");
        }

        return token;
    }

    public void DeleteLightsByToken(string token)
    {
        var lightsToDelete = _entityContext.Lights.Where(l => l.CT != token).ToList();
        _entityContext.Lights.RemoveRange(lightsToDelete);
        _entityContext.SaveChanges();
    }
}
