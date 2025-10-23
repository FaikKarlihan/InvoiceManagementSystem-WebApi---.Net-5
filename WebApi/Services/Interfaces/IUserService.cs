using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;

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

        // Queries
        Task<UserDetailViewModel> GetUserDetailAsync(string mail);
        Task<List<InvoiceDetailViewModel>> GetUserInvoicesAsync(int id);
        Task<HousingDetailViewModel> GetUserHousingDetailAsync(int id);
       
        Task<List<UserDetailViewModel>> GetAllUsersAsync(); 
        Task<UserWithHousingViewModel> GetUserWithHousingAsync(string mail);
        Task<UserWithInvoiceViewModel> GetUserWithInvoicesAsync(string mail);
        Task<UserWithAllDetailsViewModel> GetUserWithAllDetailsAsync(string mail);

    }
}