using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Authentication;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ImsDbContext _context; // We tolerate DB operations in the service since only one operation is performed.
        private readonly TokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo, ImsDbContext context, TokenGenerator tokenGenerator)
        {
            _userRepo = userRepo;
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> AuthenticateAsync(string mail, string passwordHash)
        {
            // find user in db
            var user = await _userRepo.GetByMailAsync(mail);

            if (user == null)
                throw new InvalidOperationException("User not found. Please check your email.");

            // Create token
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordHash);

            if (verificationResult == PasswordVerificationResult.Failed)
                throw new InvalidOperationException("Password is incorrect.");

            var token = _tokenGenerator.GenerateToken(user);

            return new LoginResponse
            {
                Mail = user.Mail,
                Role = user.Role.ToString(),
                AccessToken = token
            };
        }

        public async Task LogoutAsync(string token)
        {
            var revoked = new RevokedToken
            {
                Token = token,
                RevokedAt = DateTime.UtcNow
            };

            // this one
            _context.RevokedTokens.Add(revoked); 
            await _context.SaveChangesAsync();
        }
    }
}
