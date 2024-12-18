using ChristmasWorkshop.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace ChristmasWorkshop.BLL.Middleware
{
    public class TokenCaptureMiddleware
    {
        private readonly RequestDelegate next;
        private string currentToken = string.Empty;

        public TokenCaptureMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string newToken = string.Empty;

            using var scope = context.RequestServices.CreateScope();

            var tokenService = scope.ServiceProvider.GetRequiredService<TokenService>();

            // Extract token from POST header body
            if (context.Request.Method == HttpMethods.Post && context.Request.Headers.ContainsKey("Christmas-Token"))
            {
                newToken = context.Request.Headers["Christmas-Token"] !;
            }

            if (!string.IsNullOrWhiteSpace(newToken) && newToken != this.currentToken)
            {
                this.currentToken = newToken;
                tokenService.DeleteLightsByToken(this.currentToken);
            }

            // Call the next middleware in the pipeline
            await this.next(context);
        }
    }
}