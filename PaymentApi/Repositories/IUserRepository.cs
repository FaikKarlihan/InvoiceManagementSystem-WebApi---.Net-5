using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentApi.Models;

namespace PaymentApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int userId);
        Task CreateAsync(User user);
        Task UpdateAsync(int userId, User user);
        Task DeleteAsync(int userId);
    }
}