using MongoDB.Driver;
using PaymentApi.Models;
using PaymentApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMongoCollection<Transaction> _transactions;
        private readonly IMongoCollection<PaymentRequest> _paymentRequests;
        
        public UserService(IUserRepository userRepository, IMongoDatabase database)
        {
            _userRepository = userRepository;
            _transactions = database.GetCollection<Transaction>("Transactions");
            _paymentRequests = database.GetCollection<PaymentRequest>("PaymentRequests");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task CreateAsync(User user)
        {
            if (user.CardNumber.Length < 15 || user.CardNumber.Length > 16)
                throw new InvalidOperationException("Card number must be between 15 and 16 digits.");
            if (user.Password.Length != 4)
                throw new InvalidOperationException("Password must be exactly 4 digits.");

            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(int userId, User user)
        {
            await _userRepository.UpdateAsync(userId, user);
        }

        public async Task DeleteAsync(int userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<bool> DecreaseBalanceAsync(int userId, decimal amount)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            if (user.Balance < amount)
                throw new InvalidOperationException("Amount greater than balance!"); // yetersiz bakiye

            user.Balance -= amount;
            await _userRepository.UpdateAsync(userId, user);
            return true;
        }

        public async Task<bool> IncreaseBalanceAsync(int userId, decimal amount)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.Balance += amount;
            await _userRepository.UpdateAsync(userId, user);
            return true;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactions.Find(_ => true).SortByDescending(t=> t.TransactionDate).ToListAsync();
        }
        public async Task<IEnumerable<PaymentRequest>> GetAllPaymentRequestsAsync()
        {
            return await _paymentRequests.Find(_ => true).SortByDescending(t=> t.CreatedAt).ToListAsync();
        }
    }
}
