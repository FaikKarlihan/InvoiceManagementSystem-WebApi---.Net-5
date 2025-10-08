using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IUserService
    {
        // Commands
        Task<string> CreateUserAsync(UserCreateDto dto);
        Task UpdateUserAsync(UserUpdateDto dto);
        Task DeleteUserAsync(string mail);
        Task AssignHousingAsync(string mail, int id);
        Task SendMessageAsync(UserMessageDto dto, int id);

        // Queries
        Task<List<UserDetailViewModel>> GetAllUsersAsync();
        Task<UserDetailViewModel> GetUserDetailAsync(int id);
        Task<UserWithHousingViewModel> GetUserWithHousingAsync(int id);
        Task<UserWithInvoiceViewModel> GetUserWithInvoicesAsync(int id);
        Task<UserWithAllDetailsViewModel> GetUserWithAllDetailsAsync(int id);

    }
}