using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Repositories;
using WebApi.Helpers;
using WebApi.Common;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        // Commands
        public async Task<string> CreateUserAsync(UserCreateDto dto)
        {
            if (await _userRepo.UserExistsAsync(dto.Mail))
                throw new InvalidOperationException("This email already exists.");

            // map dto to user 
            var user = _mapper.Map<User>(dto);

            string rawPassword = HelperMethods.GenerateRandomPassword();
            user.PasswordHash = _passwordHasher.HashPassword(user, rawPassword);

            if (!Enum.TryParse(dto.Role, true, out Role role))
                throw new InvalidOperationException("Invalid role value.");
            user.Role = role;

            await _userRepo.AddAsync(user);
            
            return rawPassword;
        }
        public async Task UpdateUserAsync(UserUpdateDto dto)
        {
            throw new System.NotImplementedException();
        }
        public async Task DeleteUserAsync(string mail)
        {
            throw new System.NotImplementedException();
        }
        public async Task AssignHousingAsync(string mail, int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task SendMessageAsync(UserMessageDto dto, int id)
        {
            throw new System.NotImplementedException();
        }




        // Queries
        public async Task<List<UserDetailViewModel>> GetAllUsersAsync()
        {
            throw new System.NotImplementedException();
        }
        public async Task<UserDetailViewModel> GetUserDetailAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task<UserWithHousingViewModel> GetUserWithHousingAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task<UserWithInvoiceViewModel> GetUserWithInvoicesAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task<UserWithAllDetailsViewModel> GetUserWithAllDetailsAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}