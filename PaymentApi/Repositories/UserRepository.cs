using MongoDB.Driver;
using PaymentApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentApi.Repositories // inmemory yaz
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        // Constructor: MongoDB bağlantısı buradan alınıyor
        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        // Tüm kullanıcıları getir
        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        // Id'ye göre kullanıcı getir
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        // Yeni kullanıcı ekle
        public async Task CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        // Kullanıcı güncelle
        public async Task UpdateAsync(int userId, User user)
        {
            await _users.ReplaceOneAsync(u => u.UserId == userId, user);
        }

        // Kullanıcı sil
        public async Task DeleteAsync(int userId)
        {
            await _users.DeleteOneAsync(u => u.UserId == userId);
        }
    }
}
