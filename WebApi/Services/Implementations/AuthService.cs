using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApi.Authentication;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo, TokenGenerator tokenGenerator)
        {
            _userRepo = userRepo;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> AuthenticateAsync(string mail, string passwordHash)
        {
            // find user in db
            var user = await _userRepo.GetByMailAsync(mail, true);

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
    }
}
