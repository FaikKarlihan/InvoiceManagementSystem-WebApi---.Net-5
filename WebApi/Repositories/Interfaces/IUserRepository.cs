using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetWithAllDetailsByMailAsync(string mail);
        Task<bool> UserExistsAsync(string mail);
        Task<User> GetByMailAsync(string mail, bool tracked = false);
        Task AddMessageAsync(Message message);
        Task SavePasswordAsync(User user);
        Task<List<User>> GetAllWithHousings();
        Task AddPaymentAsync(Payment payment);
        Task<List<Payment>> GetUserPaymentsAsync(string mail);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<List<Message>> GetAllMessagesAsync();
    }
}