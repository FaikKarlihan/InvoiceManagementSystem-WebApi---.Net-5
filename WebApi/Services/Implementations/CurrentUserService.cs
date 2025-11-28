using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId { get; }
        public string Email { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;

            var idClaim = user.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            if (int.TryParse(idClaim, out var id))
                UserId = id;
                    
            Email = user.FindFirst(JwtRegisteredClaimNames.Email).Value;
        }
    }
}