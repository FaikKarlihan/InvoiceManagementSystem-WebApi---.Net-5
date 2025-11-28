using PaymentApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentApi.Services
{
    public interface IUserService
    {
        // Tüm kullanıcıları getir
        Task<IEnumerable<User>> GetAllAsync();

        // ID’ye göre kullanıcı getir
        Task<User> GetByIdAsync(int userId);

        // Yeni kullanıcı ekle
        Task CreateAsync(User user);

        // Kullanıcı bilgilerini güncelle (örneğin bakiye)
        Task UpdateAsync(int userId, User user);

        // Kullanıcı sil
        Task DeleteAsync(int userId);

        // Kullanıcı bakiyesini azalt (ödeme sonrası)
        Task<bool> DecreaseBalanceAsync(int userId, decimal amount);

        // Kullanıcı bakiyesini artır (örneğin iade durumu)
        Task<bool> IncreaseBalanceAsync(int userId, decimal amount);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<PaymentRequest>> GetAllPaymentRequestsAsync();
    }
}
