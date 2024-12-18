using ChristmasWorkshop.DAL.Data;
using Microsoft.AspNetCore.Http;

public class TokenService
{
    private readonly EntityContext entityContext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public TokenService(EntityContext entityContext, IHttpContextAccessor httpContextAccessor)
    {
        this.entityContext = entityContext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentToken()
    {
        var httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        string token = httpContext.Request.Headers["Christmas-Token"] !;
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("Missing Christmas-Token in request headers.");
        }

        return token;
    }

    public void DeleteLightsByToken(string token)
    {
        var lightsToDelete = this.entityContext.Lights.Where(l => l.CT != token).ToList();
        this.entityContext.Lights.RemoveRange(lightsToDelete);
        this.entityContext.SaveChanges();
    }
}
