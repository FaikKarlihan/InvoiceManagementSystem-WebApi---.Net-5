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
            // Authorization header var mı kontrol et
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Replace("Bearer ", "").Trim();

                // Token revokedTokens tablosunda var mı?
                var isRevoked = await dbContext.RevokedTokens
                                               .AsNoTracking()
                                               .AnyAsync(x => x.Token == token);

                if (isRevoked)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been revoked.");
                    return; // request'i durdur
                }
            }

            // Token geçerli veya header yok → request devam etsin
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