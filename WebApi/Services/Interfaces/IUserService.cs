using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Services
{
    public interface IUserService
    {
        // Commands
        Task<string> UserCreateAsync(UserCreateDto dto);
        Task UserUpdateAsync(UserUpdateDto dto, string userMail);
        Task UserDeleteAsync(string mail);
        Task AssignHousingAsync(string mail, string apartmentNumber, bool isOwner);
        Task SendMessageAsync(UserMessageDto dto, int id);
        Task ChangePasswordAsync(string newPassword, string mail);
        Task MakePaymentAsync(int InvoiceId, int userId);
        Task AddPaymentAsync(TransactionDto dto);

        // Queries
        Task<UserDetailViewModel> GetUserDetailAsync(string mail);
        Task<List<InvoiceDetailViewModel>> GetUserInvoicesAsync(string mail);
        Task<HousingDetailViewModel> GetUserHousingDetailAsync(string mail);
       
        Task<List<UserDetailViewModel>> GetAllUsersAsync(); 
        Task<UserWithHousingViewModel> GetUserWithHousingAsync(string mail);
        Task<UserWithInvoiceViewModel> GetUserWithInvoicesAsync(string mail);
        Task<UserWithAllDetailsViewModel> GetUserWithAllDetailsAsync(string mail);
        Task<List<Payment>> GetUserPaymentsAsync(string mail);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<List<MessagesDto>> GetAllMessagesAsync();
    }
}