using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Middlewares
{
    public class RevokedTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RevokedTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ImsDbContext dbContext)
        {
            // Check if there is an authorization header
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Replace("Bearer ", "").Trim();

                // Is the token in the revokedTokens table?
                var isRevoked = await dbContext.RevokedTokens
                                               .AsNoTracking()
                                               .AnyAsync(x => x.Token == token);

                if (isRevoked)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"error\": \"Token has been revoked.\"}");
                    return;
                }
            }

            // Token is valid or header is missing → continue request
            await _next(context);
        }

    }
    public static class RevokedTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseRevokedTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RevokedTokenMiddleware>();
        }
    }
}