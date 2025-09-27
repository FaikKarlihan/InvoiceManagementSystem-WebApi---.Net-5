using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Authentication;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ImsDbContext _context;
        private readonly TokenGenerator _tokenGenerator;

        public AuthService(ImsDbContext context, TokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            // find user in db
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == username && u.Password == password);

            if (user == null)
                throw new InvalidOperationException("User not found. Check the username and password.");

            // Create token
            var accessToken = _tokenGenerator.GenerateToken(user);

            return new LoginResponse { AccessToken = accessToken };
        }

        public async Task LogoutAsync(string token)
        {
            var revoked = new RevokedToken
            {
                Token = token,
                RevokedAt = DateTime.UtcNow
            };

            _context.RevokedTokens.Add(revoked);
            await _context.SaveChangesAsync();
        }
    }
}
