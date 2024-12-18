using ChristmasWorkshop.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChristmasWorkshop.BLL.Middleware
{
    public class TokenCaptureMiddleware
    {
        private readonly RequestDelegate _next;
        private string _currentToken = "";

        public TokenCaptureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string newToken = "";

            using (var scope = context.RequestServices.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetRequiredService<TokenService>();

                // Extract token from POST header body
                if (context.Request.Method == HttpMethods.Post && context.Request.Headers.ContainsKey("Christmas-Token"))
                {
                    newToken = context.Request.Headers["Christmas-Token"]!;
                }

                if (!string.IsNullOrWhiteSpace(newToken) && newToken != _currentToken)
                {
                    _currentToken = newToken;
                    tokenService.DeleteLightsByToken(_currentToken);
                }

                // Call the next middleware in the pipeline
                await _next(context);

            }
        }
    }
}