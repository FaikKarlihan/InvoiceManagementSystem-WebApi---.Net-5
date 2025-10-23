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
        private readonly IHousingRepository _housingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository _invoiceRepo;
        public UserService(
                            IHousingRepository housingRepository,
                            IUserRepository userRepository,
                            IPasswordHasher<User> passwordHasher,
                            IMapper mapper,
                            IInvoiceRepository invoiceRepository)
        {
            _housingRepo = housingRepository;
            _userRepo = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _invoiceRepo = invoiceRepository;
        }


        // Commands
        public async Task<string> UserCreateAsync(UserCreateDto dto)
        {
            if (await _userRepo.UserExistsAsync(dto.Mail.Trim()))
                throw new InvalidOperationException("This email already exists.");

            var user = _mapper.Map<User>(dto);
            user.NumberPlate = string.IsNullOrWhiteSpace(dto.NumberPlate) ? "-" : dto.NumberPlate;

            string rawPassword = HelperMethods.GenerateRandomPassword();
            user.PasswordHash = _passwordHasher.HashPassword(user, rawPassword);

            if (!Enum.TryParse(dto.Role, true, out Role role))
                throw new InvalidOperationException("Invalid role value.");
            user.Role = role;

            await _userRepo.AddAsync(user);

            return rawPassword;
        }
        public async Task UserUpdateAsync(UserUpdateDto dto, string userMail)
        {
            if (string.IsNullOrWhiteSpace(userMail))
                throw new InvalidOperationException("userMail cannot be empty.");

            var user = await _userRepo.GetByMailAsync(userMail);
            if (user is null)
                throw new InvalidOperationException("No user found with this email.");

            if (!string.IsNullOrWhiteSpace(dto.Mail))
            {
                if (await _userRepo.UserExistsAsync(dto.Mail))
                    throw new InvalidOperationException("EMail already exists");
            }

            Role? role = null;
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                if (!Enum.TryParse(dto.Role.Trim(), true, out Role ParsedRole))
                    throw new InvalidOperationException("Invalid role value.");
                role = ParsedRole;
            }

            if(role.HasValue)
                user.Role = role.Value;

            _mapper.Map(dto, user);
            await _userRepo.UpdateAsync(user);
        }
        public async Task UserDeleteAsync(string mail)
        {
            if(string.IsNullOrWhiteSpace(mail))
                throw new InvalidOperationException("Mail cannot be empty.");

            var user = await _userRepo.GetByMailAsync(mail.Trim());
            if (user == null)
                throw new InvalidOperationException("No user found with this email.");

            await _userRepo.DeleteAsync(user);
        }
        public async Task AssignHousingAsync(string mail, string apartmentNumber, bool isOwner)
        {
            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("Email or apartment number cannot be blank.");

            var user = await _userRepo.GetByMailAsync(mail.Trim());
            if (user == null)
                throw new InvalidOperationException("No user found with this email.");

            var housing = await _housingRepo.GetByApartmentNumberAsync(apartmentNumber.Trim());

            if (housing == null)
                throw new InvalidOperationException("There is no housing in this apartment number.");
                
            if (await _housingRepo.IsOccupiedAsync(apartmentNumber.Trim()))
                throw new InvalidOperationException("This housing is already occupied.");

            await _housingRepo.AssignToHousingAsync(user, housing, isOwner);
        }
        public async Task SendMessageAsync(UserMessageDto dto, int id) // ID is taken from token.
        {
            var message = _mapper.Map<Message>(dto);
            message.SenderId = id;
            message.SentAt = DateTime.Now;
            await _userRepo.AddMessageAsync(message);
        }
        public async Task ChangePasswordAsync(string newPassword, string mail)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                throw new InvalidOperationException("The password cannot be blank and shorter than 6 digits.");

            var user = await _userRepo.GetByMailAsync(mail);
            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

            await _userRepo.SavePasswordAsync(user);
        }


        // Queries - mail is taken from token.
        public async Task<List<UserDetailViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<List<UserDetailViewModel>>(users);
        }
        public async Task<HousingDetailViewModel> GetUserHousingDetailAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user.Housing == null)
                throw new InvalidOperationException("You have not been assigned to a housing yet.");
            var housing = await _housingRepo.GetByIdAsync(user.HousingId.Value);                
            return _mapper.Map<HousingDetailViewModel>(housing);
        }
        public async Task<List<InvoiceDetailViewModel>> GetUserInvoicesAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user.Housing == null)
                throw new InvalidOperationException("You have not been assigned to a housing yet.");

            var invoices = await _invoiceRepo.GetWhereAsync(x => x.HousingId == user.HousingId);
            return _mapper.Map<List<InvoiceDetailViewModel>>(invoices);
        }
        public async Task<UserDetailViewModel> GetUserDetailAsync(string mail)
        {
            var user = await _userRepo.GetByMailAsync(mail);
            return _mapper.Map<UserDetailViewModel>(user);
        }
        public async Task<UserWithHousingViewModel> GetUserWithHousingAsync(string mail)
        {
            var userHousing = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (userHousing.Housing == null)
                throw new InvalidOperationException("You have not been assigned to a housing yet.");

            return _mapper.Map<UserWithHousingViewModel>(userHousing);
        }
        public async Task<UserWithInvoiceViewModel> GetUserWithInvoicesAsync(string mail)
        {
            var userInvoices = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (userInvoices.Housing == null)
                throw new InvalidOperationException("You have not been assigned to a housing yet.");

            return _mapper.Map<UserWithInvoiceViewModel>(userInvoices);
        }
        public async Task<UserWithAllDetailsViewModel> GetUserWithAllDetailsAsync(string mail)
        {
            var userAllDetail = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (userAllDetail.Housing == null)
                throw new InvalidOperationException("You have not been assigned to a housing yet.");

            return _mapper.Map<UserWithAllDetailsViewModel>(userAllDetail);
        }
    }
}