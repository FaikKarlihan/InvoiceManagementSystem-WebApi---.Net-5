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
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IHousingRepository _housingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public UserService(
                            IHousingRepository housingRepository,
                            IUserRepository userRepository,
                            IPasswordHasher<User> passwordHasher,
                            IMapper mapper,
                            IInvoiceRepository invoiceRepository,
                            HttpClient httpClient,
                            IConfiguration configuration)
        {
            _housingRepo = housingRepository;
            _userRepo = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _invoiceRepo = invoiceRepository;
            _httpClient = httpClient;
            _configuration = configuration;
        }


        // COMMANDS

        // -----Admin
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

            return "Password: "+rawPassword;
        }
        public async Task UserUpdateAsync(UserUpdateDto dto, string userMail)
        {
            if (string.IsNullOrWhiteSpace(userMail))
                throw new InvalidOperationException("userMail cannot be empty.");

            var user = await _userRepo.GetByMailAsync(userMail.Trim(), true);
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
            if (string.IsNullOrWhiteSpace(mail))
                throw new InvalidOperationException("Mail cannot be empty.");

            var user = await _userRepo.GetByMailAsync(mail.Trim(), true);
            if (user is null)
                throw new InvalidOperationException("No user found with this email.");

            if (user.Housing is not null)
                    user.Housing.IsOwner = null;

            await _userRepo.DeleteAsync(user);
        }
        public async Task AssignHousingAsync(string mail, string apartmentNumber, bool isOwner)
        {
            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("Email or apartment number cannot be blank.");

            var user = await _userRepo.GetByMailAsync(mail.Trim(), true);
            if (user == null)
                throw new InvalidOperationException("No user found with this email.");

            var housing = await _housingRepo.GetByApartmentNumberAsync(apartmentNumber.Trim(), true);

            if (housing == null)
                throw new InvalidOperationException("There is no housing in this apartment number.");

            if (await _housingRepo.IsOccupiedAsync(apartmentNumber.Trim()))
                throw new InvalidOperationException("This housing is already occupied.");

            await _housingRepo.AssignToHousingAsync(user, housing, isOwner);
        }
        // -----Admin
        

        // -----User
        public async Task SendMessageAsync(UserMessageDto dto, int id)
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

            if (string.IsNullOrWhiteSpace(mail))
                throw new InvalidOperationException("Mail cannot be empty.");

            var user = await _userRepo.GetByMailAsync(mail.Trim(), true);
            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword.Trim());

            await _userRepo.SavePasswordAsync(user);
        }
        public async Task MakePaymentAsync(int InvoiceId, int userId)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(InvoiceId);
            if (invoice is null)
                throw new InvalidOperationException("Invoice not found.");
            if (invoice.PaymentStatus == PaymentStatus.Paid)
                throw new InvalidOperationException("The invoice has already been paid.");

            var initiateDto = new InitiatePaymentDto
            {
                UserId = userId,
                InvoiceId = InvoiceId,
                Amount = invoice.Amount
            };

            var url = _configuration["PaymentApi:BaseUrl"] + "/api/Payment/initiate";
 
            _httpClient.DefaultRequestHeaders.Remove("X-Api-Key"); // varsa kaldır
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _configuration["ApiKeys:PaymentApiKey"]); // yeniden ekle
            
            var response = await _httpClient.PostAsJsonAsync(url, initiateDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Payment API failed: {error}");
            }
        } 
        public async Task AddPaymentAsync(TransactionDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            var invoice = await _invoiceRepo.GetByIdAsync(dto.InvoiceId);
        
            invoice.PaymentStatus = PaymentStatus.Paid;
            await _invoiceRepo.UpdateAsync(invoice);

            var payment = new Payment
            {
                PaymentDate = dto.TransactionDate,
                InvoiceId = dto.InvoiceId,
                Amount = dto.Amount,
                UserName = user.Name + " " + user.Surname,
                UserMail = user.Mail
            };
           await _userRepo.AddPaymentAsync(payment);
        }
        // -----User

        // QUERIES

        // -----Admin
        public async Task<List<UserDetailViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllWithHousings();
            return _mapper.Map<List<UserDetailViewModel>>(users);
        }
        public async Task<UserWithHousingViewModel> GetUserWithHousingAsync(string mail)
        {
            var userHousing = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (userHousing is null)
                throw new InvalidOperationException("User not found.");
            if (userHousing.Housing == null)
                throw new InvalidOperationException("The user has not yet been assigned a housing.");

            return _mapper.Map<UserWithHousingViewModel>(userHousing);
        }
        public async Task<UserWithInvoiceViewModel> GetUserWithInvoicesAsync(string mail)
        {
            var userInvoices = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (userInvoices is null)
                throw new InvalidOperationException("User not found.");

            if (userInvoices.Housing == null)
                throw new InvalidOperationException("The user has not yet been assigned a housing.");

            return _mapper.Map<UserWithInvoiceViewModel>(userInvoices);
        }
        public async Task<UserWithAllDetailsViewModel> GetUserWithAllDetailsAsync(string mail)
        {
            var userAllDetail = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (userAllDetail is null)
                throw new InvalidOperationException("User not found.");

            if (userAllDetail.Housing == null)
                throw new InvalidOperationException("The user has not yet been assigned a housing.");

            return _mapper.Map<UserWithAllDetailsViewModel>(userAllDetail);
        }
        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var payments = await _userRepo.GetAllPaymentsAsync();
            if (!payments.Any())
                throw new InvalidOperationException("No payments found.");
            return payments;
        }
        public async Task<List<MessagesDto>> GetAllMessagesAsync()
        {
            var messages = await _userRepo.GetAllMessagesAsync();
            return _mapper.Map<List<MessagesDto>>(messages);
        }
        
        // -----Admin


        // -----User
        public async Task<HousingDetailViewModel> GetUserHousingDetailAsync(string mail)
        {
            var user = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (user.Housing == null)
                throw new InvalidOperationException("The user has not yet been assigned a housing.");
            var housing = await _housingRepo.GetWithAllDetailsByApartmentNumberAsync(user.Housing.ApartmentNumber);
            return _mapper.Map<HousingDetailViewModel>(housing);
        }
        public async Task<List<InvoiceDetailViewModel>> GetUserInvoicesAsync(string mail)
        {
            var user = await _userRepo.GetWithAllDetailsByMailAsync(mail);
            if (user.Housing == null)
                throw new InvalidOperationException("The user has not yet been assigned a housing.");

            var invoices = await _invoiceRepo.GetWhereAsync(x => x.HousingId == user.Housing.Id, includes: i => i.Housing);
            return _mapper.Map<List<InvoiceDetailViewModel>>(invoices);
        }
        public async Task<UserDetailViewModel> GetUserDetailAsync(string mail)
        {
            var user = await _userRepo.GetByMailAsync(mail);
            if (user is null)
                throw new InvalidOperationException("User not found.");
            return _mapper.Map<UserDetailViewModel>(user);
        }
        public async Task<List<Payment>> GetUserPaymentsAsync(string mail)
        {
            var payments = await _userRepo.GetUserPaymentsAsync(mail);
            if (!payments.Any())
                throw new InvalidOperationException("No payments found.");
            return payments;
        }
        // -----User
    }
}