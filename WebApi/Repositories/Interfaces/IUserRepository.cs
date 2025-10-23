using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetWithAllDetailsByMailAsync(string mail);
        Task<bool> UserExistsAsync(string mail);
        Task<User> GetByMailAsync(string mail);
        Task AddMessageAsync(Message message);
        Task SavePasswordAsync(User user);
    }
}