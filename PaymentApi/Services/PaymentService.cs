using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentApi.Models;
using PaymentApi.Models.DTOs;
using PaymentApi.Repositories;
using PaymentApi.Services;

public class PaymentService
{
    private readonly IMongoCollection<PaymentRequest> _paymentRequests;
    private readonly IMongoCollection<Transaction> _transactions;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public PaymentService(IMongoDatabase database, IUserRepository userRepository, IUserService userService)
    {
        _paymentRequests = database.GetCollection<PaymentRequest>("PaymentRequests");
        _transactions = database.GetCollection<Transaction>("Transactions");
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<string> InitiatePaymentAsync(InitiatePaymentDto dto)
    {
        if (await _userRepository.GetByIdAsync(dto.UserId) == null)
            throw new InvalidOperationException("User not found.");

        var request = new PaymentRequest
        {
            UserId = dto.UserId,
            InvoiceId = dto.InvoiceId,
            Amount = dto.Amount
        };

        await _paymentRequests.InsertOneAsync(request);
        return request.PaymentToken;
    }

    public async Task<Transaction> ConfirmPaymentAsync(ConfirmPaymentDto dto)
    {
        var paymentRequest = await _paymentRequests
            .Find(x => x.PaymentToken == dto.PaymentToken && !x.IsCompleted)
            .FirstOrDefaultAsync();

        if (paymentRequest == null)
            throw new Exception("Payment request not found or already completed.");

        // Simülasyon olarak ProcessPayment çağrısı
        var transaction = await ProcessPaymentAsync(paymentRequest, dto);

        paymentRequest.IsCompleted = true;
        await _paymentRequests.ReplaceOneAsync(x => x.Id == paymentRequest.Id, paymentRequest);

        return transaction;
    }

    private async Task<Transaction> ProcessPaymentAsync(PaymentRequest paymentDto, ConfirmPaymentDto confirmDto)
    {
        // Kullanıcıyı al
        var user = await _userRepository.GetByIdAsync(paymentDto.UserId);
        if (user == null)
            throw new Exception("User not found");
               
        // Kart doğrulama
        if (user.CardNumber != confirmDto.CardNumber || user.Password != confirmDto.CardPassword)
            throw new Exception("Invalid card credentials");

        // Bakiye kontrolü
        if (user.Balance < paymentDto.Amount)
            throw new Exception("Insufficient balance");

        // Bakiye düş
        await _userService.DecreaseBalanceAsync(user.UserId, paymentDto.Amount);
             
        // Transaction objesi oluştur
        var transaction = new Transaction
        {
            UserId = paymentDto.UserId,
            InvoiceId = paymentDto.InvoiceId,
            Amount = paymentDto.Amount,
            TransactionDate = DateTime.UtcNow,
        };
           
        await _transactions.InsertOneAsync(transaction);
             
        return transaction; // Ana API’ye gönderilecek
    }
}