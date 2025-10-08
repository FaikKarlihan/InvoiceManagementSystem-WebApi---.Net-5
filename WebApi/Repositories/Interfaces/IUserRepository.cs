using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetWithAllDetailsByIdAsync(int id);
        Task<bool> UserExistsAsync(string Mail);
        Task<User> GetByMailAsync(string mail);
    }
}